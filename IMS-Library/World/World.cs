using KinglyStudios.Knetworking;
using System;
using System.Collections.Generic;
using System.IO;

namespace IMS_Library
{
    public class World : IMSConfiguration
    {
        public Guid ID;
        public string Name;
        public WorldType WorldEdition;

        public string FolderPath { get => Constants.WorldFolderLocation + "/" + ID; }
        public string WorldPath { get {
                string toReturn = FolderPath + "/world";
                if (!Directory.Exists(toReturn))
                {
                    Directory.CreateDirectory(toReturn);
                }
                return toReturn;
            }
        }

        public World(Guid id)
        {
            ID = id;
        }

        public override string GetDefaultFilePath()
        {
            return FolderPath + "/config.xml";
        }

        public long GetTotalSize()
        {
            return GetDirectorySize(new DirectoryInfo(FolderPath));
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
