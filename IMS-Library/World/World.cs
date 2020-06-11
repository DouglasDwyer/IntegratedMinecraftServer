using KinglyStudios.Knetworking;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IMS_Library
{
    /// <summary>
    /// Instances of this class represent Minecraft worlds, allowing for control over the world files and their backups.
    /// </summary>
    public sealed class World : IMSConfiguration
    {
        /// <summary>
        /// The unique identifier of the world.
        /// </summary>
        public Guid ID;
        /// <summary>
        /// The display name of the world.
        /// </summary>
        public string Name;
        /// <summary>
        /// The edition of Minecraft that this world is associated with.
        /// </summary>
        public MinecraftEdition Edition;
        /// <summary>
        /// The backup policies that this world utilizes to make/delete backups.
        /// </summary>
        public SynchronizedCollection<IBackupPolicy> BackupPolicies = new SynchronizedCollection<IBackupPolicy>();
        /// <summary>
        /// All of the backups that currently exist of this world, indexed by unique identifier.
        /// </summary>
        public ConcurrentDictionary<Guid, BackupInformation> Backups = new ConcurrentDictionary<Guid, BackupInformation>();

        /// <summary>
        /// The absolute path of this world's directory (the directory which stores metadata, world backups, and the current world files).  This is the parent directory of <see cref="WorldPath"/>.
        /// </summary>
        public string FolderPath { get => Constants.ExecutionPath + Constants.WorldFolderLocation + "/" + ID; }
        /// <summary>
        /// The absolute path of the current Minecraft world folder.
        /// </summary>
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
        /// <summary>
        /// The absolute path of the world's display icon.  This always returns a default path based on world edition - it does not check to see whether an icon exists.
        /// </summary>
        public string IconPath {
            get { 
                if(Edition == MinecraftEdition.Java)
                {
                    return WorldPath + "/icon.png";
                }
                else if(Edition == MinecraftEdition.Bedrock)
                {
                    return WorldPath + "/world_icon.jpeg";
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="World"/> class, generating a new GUID to associate it with.
        /// </summary>
        public World() {
            ID = Guid.NewGuid();
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="World"/> class with the specified GUID.
        /// </summary>
        /// <param name="id">The unique identifier to associate this world with.</param>
        public World(Guid id)
        {
            ID = id;
        }

        /// <summary>
        /// Takes a snapshot of the current world.
        /// </summary>
        /// <param name="backupName">The display name of the backup to create.</param>
        /// <returns>The unique identifier of the backup that was created.</returns>
        public async Task<Guid> MakeBackupAsync(string backupName = "Automatic backup")
        {
            Guid backupID = Guid.NewGuid();
            Backups[backupID] = new BackupInformation { Name = backupName, Date = DateTime.Now, ID = backupID };
            ServerProxy server = IMS.Instance.WorldManager.GetServerOfWorld(this);
            if (server is null || server.State == ServerProxy.ServerState.Disabled)
            {
                Extensions.CopyFolder(WorldPath, FolderPath + "/" + backupID);
            }
            else
            {
                await server.BackupToLocationAsync(FolderPath + "/" + backupID);
            }
            return backupID;
        }

        /// <summary>
        /// Restores a world to a former backup.
        /// </summary>
        /// <param name="backupID">The unique identifier of the backup to restore to.</param>
        /// <param name="makeBackupOfCurrentWorld">Whether the current Minecraft world should be backed up before restoration, or simply overwritten.</param>
        /// <param name="currentWorldBackupName">The name of the backup that represents the replaced Minecraft world instance.  This is only used if <paramref name="makeBackupOfCurrentWorld"/> is <c>true</c>.</param>
        /// <returns>A <see cref="Task"/> object representing the progress of the operation, including any necessary server restart and the copying of the Minecraft world folders.</returns>
        public async Task RestoreFromBackupAsync(Guid backupID, bool makeBackupOfCurrentWorld = true, string currentWorldBackupName = "Overwritten world backup")
        {
            if (!Backups.ContainsKey(backupID))
            {
                throw new ArgumentException("Backup ID did not match any known backup.", "backupID");
            }
            ServerProxy server = IMS.Instance.WorldManager.GetServerOfWorld(this);
            if (server != null)
            {
                if (server.State == ServerProxy.ServerState.Disabled)
                {
                    server = null;
                }
                else
                {
                    await server.StopAsync();
                }
            }
            if (makeBackupOfCurrentWorld)
            {
                await MakeBackupAsync(currentWorldBackupName);
            }
            await Task.Run(() =>
            {
                Directory.Delete(WorldPath, true);
                Extensions.CopyFolder(FolderPath + "/" + backupID, WorldPath);
            });
            await server?.StartAsync();
        }

        /// <summary>
        /// Retrieves the absolute path of a backup's Minecraft world folder.
        /// </summary>
        /// <param name="backup">The backup to get information about.</param>
        /// <returns>The absolute path of the Minecraft world folder.</returns>
        public string GetPathOfBackup(BackupInformation backup)
        {
            if (Backups.ContainsKey(backup.ID))
            {
                return FolderPath + "/" + backup.ID;
            }
            return null;
        }

        /// <summary>
        /// Retrieves the absolute path of a backup's Minecraft world folder.
        /// </summary>
        /// <param name="backup">The unique identifier of the backup to get information about.</param>
        /// <returns>The absolute path of the Minecraft world folder.</returns>
        public string GetPathOfBackup(Guid backup)
        {
            return FolderPath + "/" + backup;
        }

        /// <summary>
        /// Deletes a backup from the backup registry, removing the Minecraft world folder from disk.
        /// </summary>
        /// <param name="backupID">The unique identifier of the backup to remove.</param>
        /// <returns>A <see cref="Task"/> object representing the current state of the directory deletion operation.</returns>
        public async Task DeleteBackupAsync(Guid backupID)
        {
            if (Backups.Remove(backupID))
            {
                await Task.Run(() => Directory.Delete(FolderPath + "/" + backupID, true));
            }
        }

        /// <summary>
        /// Retrieves the file path for this <see cref="World"/>'s configuration file.
        /// </summary>
        /// <returns>The absolute path of the configuration file.</returns>
        public override string GetDefaultFilePath()
        {
            return FolderPath + "/config.xml";
        }

        /// <summary>
        /// Calls the <see cref="IBackupPolicy.Update(World)"/> method on all registered backup policies.
        /// </summary>
        public void RunBackupUpdates()
        {
            foreach(IBackupPolicy policy in BackupPolicies)
            {
                policy.Update(this);
            }
        }

        private long GetTotalSize()
        {
            return GetDirectorySize(new DirectoryInfo(FolderPath));
        }

        /// <summary>
        /// Retrieves the total size of all Minecraft world files on disk.
        /// </summary>
        /// <returns>The size, in bytes, of the <see cref="FolderPath"/> folder.</returns>
        public async Task<long> GetTotalSizeAsync()
        {
            return await Task.Run(GetTotalSize);
        }

        /// <summary>
        /// Retrieves the total size of the current Minecraft world folder.
        /// </summary>
        /// <returns>The size, in bytes, of the <see cref="WorldPath"/> folder.</returns>
        public async Task<long> GetWorldSizeAsync()
        {
            return await Task.Run(GetWorldSize);
        }

        private long GetWorldSize()
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
    }
}
