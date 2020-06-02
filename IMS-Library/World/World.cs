﻿using KinglyStudios.Knetworking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IMS_Library
{
    public sealed class World : IMSConfiguration
    {
        public Guid ID;
        public string Name;
        public WorldType Edition;
        public List<IBackupPolicy> BackupPolicies = new List<IBackupPolicy>();
        public List<BackupInformation> Backups = new List<BackupInformation>();

        public string FolderPath { get => Constants.ExecutionPath + Constants.WorldFolderLocation + "/" + ID; }
        public string WorldPath {
            get {
                string toReturn = FolderPath + "/world";
                if (!Directory.Exists(toReturn))
                {
                    Directory.CreateDirectory(toReturn);
                }
                return toReturn;
            }
        }
        public string IconPath {
            get { 
                if(Edition == WorldType.Java)
                {
                    return WorldPath + "/icon.png";
                }
                else if(Edition == WorldType.Bedrock)
                {
                    return WorldPath + "/world_icon.jpeg";
                }
                throw new InvalidOperationException();
            }
        }

        public World() { }

        public World(Guid id)
        {
            ID = id;
        }

        public async Task<Guid> MakeBackupAsync(string backupName = "Automatic backup")
        {
            Guid backupID = Guid.NewGuid();
            ServerProxy server = IMS.Instance.WorldManager.GetServerOfWorld(this);
            Backups.Add(new BackupInformation { Name = backupName, Date = DateTime.Now, ID = backupID });
            if (server is null || server.State == ServerProxy.ServerState.Disabled) {
                await Task.Run(() =>
                {
                    Extensions.CopyFolder(WorldPath, FolderPath + "/" + backupID);
                });
            }
            else
            {
                await server.BackupToLocationAsync(FolderPath + "/" + backupID);
            }
            return backupID;
        }

        public async Task RestoreFromBackupAsync(Guid backupID, bool makeBackupOfCurrentWorld = true, string currentWorldBackupName = "Overwritten world backup")
        {
            if(Backups.FindIndex(x => x.ID == backupID) < 0)
            {
                throw new ArgumentException("Backup ID did not match any known backup.", "backupID");
            }
            ServerProxy server = IMS.Instance.WorldManager.GetServerOfWorld(this);
            if(server != null)
            {
                if(server.State == ServerProxy.ServerState.Disabled)
                {
                    server = null;
                }
                else
                {
                    server.StopAndWait();
                }
            }
            if(makeBackupOfCurrentWorld)
            {
                await MakeBackupAsync(currentWorldBackupName);
            }
            await Task.Run(() =>
            {
                Directory.Delete(WorldPath, true);
                Extensions.CopyFolder(FolderPath + "/" + backupID, WorldPath);
            });
            IMS.AsThreadSafe(() => server?.Start());
        }

        public string GetPathOfBackup(BackupInformation backup)
        {
            if (Backups.Contains(backup))
            {
                return FolderPath + "/" + backup.ID;
            }
            return null;
        }

        public string GetPathOfBackup(Guid backup)
        {
            return FolderPath + "/" + backup;
        }

        public async Task DeleteBackupAsync(Guid backupID)
        {
            BackupInformation info = Backups.Find(x => x.ID == backupID);
            if(info != null)
            {
                Backups.Remove(info);
                await Task.Run(() => Directory.Delete(FolderPath + "/" + info.ID, true));
            }
        }

        public override string GetDefaultFilePath()
        {
            return FolderPath + "/config.xml";
        }

        public void RunBackupUpdates(ServerProxy boundServer)
        {
            foreach(IBackupPolicy policy in BackupPolicies)
            {
                policy.Update(this);
            }
        }

        public long GetTotalSize()
        {
            return GetDirectorySize(new DirectoryInfo(FolderPath));
        }

        public async Task<long> GetTotalSizeAsync()
        {
            return await Task.Run(GetTotalSize);
        }

        public async Task<long> GetWorldSizeAsync()
        {
            return await Task.Run(GetWorldSize);
        }

        public long GetWorldSize()
        {
            return GetDirectorySize(new DirectoryInfo(WorldPath));
        }

        private static long GetDirectorySize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += GetDirectorySize(di);
            }
            return size;
        }

        public enum WorldType { Java, Bedrock }
    }
}
