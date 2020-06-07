using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    public class ServerVersionInformation
    {
        public string Name;
        public string Version;
        public DateTime ReleaseTime;
        public MinecraftEdition Edition;
        public ReleaseType VersionType;
        public string DownloadURL;

        private string DefaultLocation => Constants.ExecutionPath + Constants.ServerBinariesFolderLocation + "/" + Edition + "-" + Version + (Edition == MinecraftEdition.Java ? ".jar" : ".exe");
        public string PhysicalLocation => File.Exists(DefaultLocation) ? DefaultLocation : null;

        public ServerVersionInformation() { }

        public ServerVersionInformation(string name, string version, DateTime releaseTime, MinecraftEdition edition, ReleaseType versionType, string downloadUrl)
        {
            Name = name;
            Version = version;
            ReleaseTime = releaseTime;
            Edition = edition;
            VersionType = versionType;
            DownloadURL = downloadUrl;
        }

        public async Task DownloadServerBinaryAsync()
        {
            using(WebClient client = new WebClient())
            {
                string path = Path.GetDirectoryName(DefaultLocation);
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                try
                {
                    lock (DownloadURL)
                    {
                        if (PhysicalLocation != null)
                        {
                            return;
                        }
                        File.WriteAllBytes(DefaultLocation, new byte[0]);
                    }
                    await client.DownloadFileTaskAsync(DownloadURL, DefaultLocation);
                }
                catch
                {
                    if(File.Exists(DefaultLocation))
                    {
                        File.Delete(DefaultLocation);
                    }
                    throw;
                }                
            }
        }

        public enum ReleaseType { Release, Snapshot, OldBeta, OldAlpha }
    }
}
