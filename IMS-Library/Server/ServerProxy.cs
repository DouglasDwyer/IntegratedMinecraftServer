using KinglyStudios.Knetworking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    public abstract class ServerProxy
    {
        public Guid ID { get; protected set; }
        public ServerState State { get; protected set; }
        public abstract ServerConfiguration CurrentConfiguration { get; set; }

        public abstract bool SupportsWhitelist { get; }
        public abstract bool WhitelistEnabled { get; set; }
        public abstract bool SupportsBanning { get; }
        public abstract bool SupportsBanningIPs { get; }
        public abstract bool SupportsOps { get; }
        public abstract bool SupportsIPs { get; }
        public abstract bool SupportsKicking { get; }
        public abstract MinecraftEdition SupportedEdition { get; }

        public ServerProxy(Guid guid) {
            ID = guid;
        }

        public abstract Task StartAsync();
        public abstract Task StopAsync();
        public abstract Task RestartAsync();
        public abstract MinecraftPlayer GetPlayerInformationByUsername(string username);
        public abstract MinecraftPlayer GetPlayerInformationByUUID(string uuid);
        public abstract IEnumerable<MinecraftPlayer> GetOnlinePlayers();
        public abstract IEnumerable<MinecraftPlayer> GetAllPlayers();
        public abstract IEnumerable<MinecraftPlayer> GetAllOps();
        public abstract List<BanInformation> GetAllBans();
        public abstract List<BanIPTag> GetAllBannedIPs();
        public abstract IEnumerable<MinecraftPlayer> GetAllWhitelistedPlayers();
        public abstract void WhitelistPlayer(string name);
        public abstract void RemoveWhitelistPlayer(string name);
        public abstract void OpPlayer(string name);
        public abstract void DeopPlayer(string name);
        public abstract void KickPlayer(string name, string reason);
        public abstract void BanPlayer(string name, string reason);
        public abstract void BanIP(string ip, string reason);
        public abstract void UnbanPlayer(string name);
        public abstract void UnbanIP(string ip);
        public abstract void SendConsoleCommand(string command);
        public abstract string GetConsoleText();
        public abstract void ReloadServerWhitelist();
        public abstract void ReloadServerPermissions();
        public abstract List<LogFileInformation> GetLogFiles();
        public abstract string GetLogFile(string name);
        public abstract Task BackupToLocationAsync(string location);
        public abstract Task BackupToZipFileAsync(string file);

        public virtual ServerConfiguration GetDefaultServerConfiguration()
        {
            return new ServerConfiguration(ID).FromConfiguration();
        }

        public enum ServerState { Disabled, Starting, Running, Stopping }
    }
}
