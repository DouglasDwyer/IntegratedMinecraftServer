using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    public class MinecraftVersionProvider : IMSConfiguration
    {
        public ServerVersionInformation LatestRelease => AvailableServerVersions.ContainsKey(LatestReleaseID) ? AvailableServerVersions[LatestReleaseID] : null;
        public ServerVersionInformation LatestSnapshot => AvailableServerVersions.ContainsKey(LatestSnapshotID) ? AvailableServerVersions[LatestSnapshotID] : null;
        public ConcurrentDictionary<string, ServerVersionInformation> AvailableServerVersions = new ConcurrentDictionary<string, ServerVersionInformation>();
        public string LatestReleaseID;
        public string LatestSnapshotID;

        public void Start()
        {
            UpdateAllServerVersionsAsync();
        }

        public void Stop()
        {
            this.SaveConfiguration();
        }

        public ServerVersionInformation GetVersionInformationFromID(string id)
        {
            return string.IsNullOrEmpty(id) ? LatestRelease : AvailableServerVersions[id];
        }

        public override string GetDefaultFilePath()
        {
            return Constants.ExecutionPath + Constants.DataLocation + "/versioninformation.xml";
        }

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
                        AvailableServerVersions[metadata.id] = new ServerVersionInformation(name, metadata.id, DateTime.Parse(versionData.releaseTime), World.WorldType.Java, releaseType, versionData.downloads.server.url);
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
