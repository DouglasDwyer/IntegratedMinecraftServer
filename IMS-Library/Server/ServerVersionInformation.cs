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
        public World.WorldType Edition;
        public ReleaseType VersionType;
        public string DownloadURL;

        private string DefaultLocation => Constants.ExecutionPath + Constants.ServerBinariesFolderLocation + "/" + Edition + "-" + Version + (Edition == World.WorldType.Java ? ".jar" : ".exe");
        public string PhysicalLocation => File.Exists(DefaultLocation) ? DefaultLocation : null;

        public ServerVersionInformation() { }

        public ServerVersionInformation(string name, string version, DateTime releaseTime, World.WorldType edition, ReleaseType versionType, string downloadUrl)
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
                await client.DownloadFileTaskAsync(DownloadURL, DefaultLocation);
            }
        }

        public enum ReleaseType { Release, Snapshot, OldBeta, OldAlpha }
    }
}
