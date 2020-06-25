using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace IMS_Library
{
    /// <summary>
    /// This class acts as a manager for Minecraft versions.  It keeps track of Minecraft versions and updates version data automatically.
    /// </summary>
    public class MinecraftVersionProvider : IMSConfiguration
    {
        /// <summary>
        /// The latest version of Minecraft: Java Edition to be made available as an official release.
        /// </summary>
        public ServerVersionInformation LatestRelease => AvailableServerVersions.ContainsKey(LatestReleaseID) ? AvailableServerVersions[LatestReleaseID] : null;
        /// <summary>
        /// The latest version of Minecraft: Java Edition to be made available as a prerelease snapshot.
        /// </summary>
        public ServerVersionInformation LatestSnapshot => AvailableServerVersions.ContainsKey(LatestSnapshotID) ? AvailableServerVersions[LatestSnapshotID] : null;
        /// <summary>
        /// This dictionary contains information about every version of Minecraft, indexed by version code.
        /// </summary>
        public ConcurrentDictionary<string, ServerVersionInformation> AvailableServerVersions = new ConcurrentDictionary<string, ServerVersionInformation>();
        /// <summary>
        /// This is the ID of the latest release of Minecraft.
        /// </summary>
        public string LatestReleaseID = "1.15.2";
        /// <summary>
        /// This is the ID of the latest snapshot of Minecraft.
        /// </summary>
        public string LatestSnapshotID = "20w09a";

        private Timer AutomaticUpdateTimer = new Timer();

        /// <summary>
        /// Begins the <see cref="MinecraftVersionProvider"/> instance, beginning an update timer and updating version data.
        /// </summary>
        public void Start()
        {
            UpdateAllServerVersionsAsync();
            AutomaticUpdateTimer.Interval = 10 * 60 * 1000;
            AutomaticUpdateTimer.Elapsed += (x, y) => UpdateAllServerVersionsAsync();
            AutomaticUpdateTimer.Start();
        }

        /// <summary>
        /// This method attempts to restart any servers configured to "use the latest Minecraft version" that are running an outdated version.
        /// </summary>
        public void RestartUpdatedServers()
        {
            foreach (ServerProxy server in IMS.Instance.ServerManager.Servers)
            {
                if (server.State != ServerProxy.ServerState.Disabled && server.CurrentConfiguration is JavaServerConfiguration config)
                {
                    if (string.IsNullOrEmpty(config.ServerVersion) && server.ServerVersionID != LatestRelease.Version)
                    {
                        server.RestartAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Stops the <see cref="MinecraftVersionProvider"/> instance, saving all version data to disk.
        /// </summary>
        public void Stop()
        {
            this.SaveConfiguration();
            AutomaticUpdateTimer.Enabled = false;
        }

        /// <summary>
        /// Gets information about a certain Minecraft version from its version code.
        /// </summary>
        /// <param name="id">The version code of the version to get information about.</param>
        /// <returns>A <see cref="ServerVersionInformation"/> object that contains data about the server version, or null if no version was found.</returns>
        public ServerVersionInformation GetVersionInformationFromID(string id)
        {
            return string.IsNullOrEmpty(id) ? LatestRelease : AvailableServerVersions[id];
        }

        /// <summary>
        /// Retrieves the location of the version provider's settings file.
        /// </summary>
        /// <returns>An absolute path representing this instance's configuration file.</returns>
        public override string GetDefaultFilePath()
        {
            return Constants.ExecutionPath + Constants.DataLocation + "/versioninformation.xml";
        }

        /// <summary>
        /// Downloads updates about all server versions.
        /// </summary>
        /// <returns>A <see cref="Task"/> object representing the state of the current update operation.</returns>
        public async Task UpdateAllServerVersionsAsync()
        {
            VersionInformationTag versionInfo = await MojangInteropUtility.GetAllJavaVersionInformation();
            LatestReleaseID = versionInfo.latest.release;
            LatestSnapshotID = versionInfo.latest.snapshot;
            foreach(VersionMetadataTag metadata in versionInfo.versions)
            {
                if(!AvailableServerVersions.ContainsKey(metadata.id))
                {
                    VersionDataTag versionData = default;
                    try
                    {
                        versionData = await MojangInteropUtility.GetVersionInformation(metadata);
                    }
                    catch(Exception e)
                    {
                        Logger.WriteWarning("Couldn't download information for server version " + metadata.id + "!\n" + e);
                        continue;
                    }
                    if(string.IsNullOrEmpty(versionData.downloads.server.url))
                    {
                        continue;
                    }
                    string name = null;
                    ServerVersionInformation.ReleaseType releaseType = ServerVersionInformation.ReleaseType.Release;
                    switch(metadata.type)
                    {
                        case "release":
                            name = "Release ";
                            releaseType = ServerVersionInformation.ReleaseType.Release;
                            break;
                        case "snapshot":
                            name = "Snapshot ";
                            releaseType = ServerVersionInformation.ReleaseType.Snapshot;
                            break;
                        case "old_beta":
                            name = "Beta ";
                            releaseType = ServerVersionInformation.ReleaseType.OldBeta;
                            break;
                        case "old_alpha":
                            name = "Alpha ";
                            releaseType = ServerVersionInformation.ReleaseType.OldAlpha;
                            break;
                        default:
                            name = "Minecraft ";
                            break;
                    }
                    name += metadata.id;
                    lock (this)
                    {
                        AvailableServerVersions[metadata.id] = new ServerVersionInformation(name, metadata.id, DateTime.Parse(versionData.releaseTime), MinecraftEdition.Java, releaseType, versionData.downloads.server.url);
                    }
                }
            }
            if(LatestRelease.PhysicalLocation is null)
            {
                await LatestRelease.DownloadServerBinaryAsync();
            }
        }
    }
}
