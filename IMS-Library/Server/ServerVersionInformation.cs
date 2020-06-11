using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    /// <summary>
    /// Instances of this class contain information about a particular Minecraft server version.
    /// </summary>
    public class ServerVersionInformation
    {
        /// <summary>
        /// The display name of the server version.
        /// </summary>
        public string Name;
        /// <summary>
        /// The official Minecraft version code.
        /// </summary>
        public string Version;
        /// <summary>
        /// The time at which this Minecraft version was released.
        /// </summary>
        public DateTime ReleaseTime;
        /// <summary>
        /// The edition of Minecraft that this server supports.
        /// </summary>
        public MinecraftEdition Edition;
        /// <summary>
        /// The type of release that this server was made under.
        /// </summary>
        public ReleaseType VersionType;
        /// <summary>
        /// The download URL for the Minecraft server binary.
        /// </summary>
        public string DownloadURL;

        private string DefaultLocation => Constants.ExecutionPath + Constants.ServerBinariesFolderLocation + "/" + Edition + "-" + Version + (Edition == MinecraftEdition.Java ? ".jar" : ".exe");
        
        /// <summary>
        /// Represents where the server binary is located on disk.  This returns an absolute path if the server binary is downloaded, or null if the server binary does not exist locally.
        /// </summary>
        public string PhysicalLocation => File.Exists(DefaultLocation) ? DefaultLocation : null;

        /// <summary>
        /// Constructs a new <see cref="ServerVersionInformation"/> instance.
        /// </summary>
        public ServerVersionInformation() { }

        /// <summary>
        /// Constructs a new <see cref="ServerVersionInformation"/> instance with the specified information.
        /// </summary>
        /// <param name="name">The display name of the version.</param>
        /// <param name="version">The official Minecraft version code.</param>
        /// <param name="releaseTime">When the server was publicly released.</param>
        /// <param name="edition">The type of Minecraft that this server supports.</param>
        /// <param name="versionType">The release type that this server was released under.</param>
        /// <param name="downloadUrl">The URL for downloading the server binary.</param>
        public ServerVersionInformation(string name, string version, DateTime releaseTime, MinecraftEdition edition, ReleaseType versionType, string downloadUrl)
        {
            Name = name;
            Version = version;
            ReleaseTime = releaseTime;
            Edition = edition;
            VersionType = versionType;
            DownloadURL = downloadUrl;
        }

        /// <summary>
        /// Downloads the server binary from the internet if it does not already exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object representing the current state of the download operation.</returns>
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

        /// <summary>
        /// Represents the type of software release that any one version was made available under.
        /// </summary>
        public enum ReleaseType
        {
            /// <summary>
            /// This server version goes along with an official release of a new Minecraft version.
            /// </summary>
            Release,
            /// <summary>
            /// This server version goes along with a "preview" version of a new Minecraft update.
            /// </summary>
            Snapshot,
            /// <summary>
            /// This server version corresponds to a beta version of Minecraft.
            /// </summary>
            OldBeta,
            /// <summary>
            /// This server version corresponds to an alpha version of Minecraft.
            /// </summary>
            OldAlpha
        }
    }
}
