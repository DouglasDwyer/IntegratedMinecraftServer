using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IMS_Library
{
    [Serializable]
    [XmlRoot(Namespace = "IMS_Service")]
    public class JavaServerConfiguration : ServerConfiguration
    {
        //public bool Enabled = true;
        public bool UseJITCompiler = false;
        public int MinimumMemoryMB = 512;
        public int MaximumMemoryMB = 1024;
        public string JavaArguments = "-XX:+AggressiveOpts -XX:+UnlockExperimentalVMOptions";

        [ServerProperty("allow-flight")]
        public bool AllowFlight = false;
        [ServerProperty("allow-nether")]
        public bool AllowNether = true;
        [ServerProperty("broadcast-console-to-ops")]
        public bool BroadcastConsoleToOps = true;
        [ServerProperty("broadcast-rcon-to-ops")]
        public bool BroadcastRCONToOps = true;
        [ServerProperty("difficulty")]
        public int Difficulty = 1;
        [ServerProperty("enable-command-block")]
        public bool EnableCommandBlocks = false;
        [ServerProperty("enable-rcon")]
        public bool EnableRCON = false;
        [ServerProperty("sync-chunk-writes")]
        public bool SyncChunkWrites = true;
        [ServerProperty("enable-query")]
        public bool EnableQuery = false;
        [ServerProperty("force-gamemode")]
        public bool ForceGamemode = false;
        [ServerProperty("function-permission-level")]
        public int FunctionPermissionLevel = 2;
        [ServerProperty("gamemode")]
        public int Gamemode = 0;
        [ServerProperty("generate-structures")]
        public bool GenerateStructures = true;
        [ServerProperty("generator-settings")]
        public string GeneratorSettings = "";
        [ServerProperty("hardcore")]
        public bool HardcoreMode = false;
        [ServerProperty("level-name")]
        public string LevelName = "world";
        [ServerProperty("level-seed")]
        public string LevelSeed = "";
        [ServerProperty("level-type")]
        public string LevelType = "default";
        [ServerProperty("max-build-height")]
        public int MaxBuildHeight = 256;
        [ServerProperty("max-players")]
        public int MaxPlayers = 20;
        [ServerProperty("max-tick-time")]
        public int MaxTickTime = 60000;
        [ServerProperty("max-world-size")]
        public int MaxWorldSize = 29999984;
        [MOTDServerProperty("motd")]
        public string MessageOfTheDay = "";
        [ServerProperty("network-compression-threshold")]
        public int NetworkCompressionThreshold = 256;
        [ServerProperty("online-mode")]
        public bool OnlineMode = true;
        [ServerProperty("op-permission-level")]
        public int DefaultOpPermissionLevel = 4;
        [ServerProperty("player-idle-timeout")]
        public int PlayerIdleTimeout = 0;
        [ServerProperty("prevent-proxy-connections")]
        public bool PreventProxyConnections = false;
        [ServerProperty("pvp")]
        public bool EnablePVP = true;
        [ServerProperty("query.port")]
        public WebPort QueryPort = new WebPort(25565, true);
        [ServerProperty("rcon.password")]
        public string RCONPassword = "";
        [ServerProperty("rcon.port")]
        public WebPort RCONPort = new WebPort(25575, false);
        [ServerProperty("resource-pack")]
        public string ResourcePack = "";
        [ServerProperty("resource-pack-sha1")]
        public string ResourcePackSHA1 = "";
        [ServerProperty("server-ip")]
        public string ServerIP = "";
        [ServerProperty("server-port")]
        public WebPort ServerPort = new WebPort(25565, true);
        [ServerProperty("snooper-enabled")]
        public bool SnooperEnabled = true;
        [ServerProperty("spawn-animals")]
        public bool SpawnAnimals = true;
        [ServerProperty("spawn-monsters")]
        public bool SpawnMonsters = true;
        [ServerProperty("spawn-npcs")]
        public bool SpawnVillagers = true;
        [ServerProperty("spawn-protection")]
        public int SpawnProtection = 0;
        [ServerProperty("use-native-transport")]
        public bool UseLinuxNativeTransport = false;
        [ServerProperty("view-distance")]
        public int ViewDistance = 10;
        [ServerProperty("white-list")]
        public bool UseWhitelist = false;
        [ServerProperty("enforce-whitelist")]
        public bool EnforceWhitelist = false;

        public JavaServerConfiguration() { }
        public JavaServerConfiguration(Guid id) : base(id) { }

        public override int[] GetUsedPorts()
        {
            List<int> ports = new List<int>();
            ports.Add(ServerPort.Port);
            if(EnableQuery && QueryPort.Port != ServerPort.Port)
            {
                ports.Add(QueryPort.Port);
            }
            if(EnableRCON)
            {
                ports.Add(RCONPort.Port);
            }
            return ports.ToArray();
        }

        public override int[] GetPortsToForward()
        {
            List<int> ports = new List<int>();
            if(ServerPort.AttemptUPnPForwarding)
            {
                ports.Add(ServerPort.Port);
            }
            if((ServerPort.Port != QueryPort.Port || !ServerPort.AttemptUPnPForwarding) && QueryPort.AttemptUPnPForwarding)
            {
                ports.Add(QueryPort.Port);
            }
            if(EnableRCON && RCONPort.AttemptUPnPForwarding)
            {
                ports.Add(RCONPort.Port);
            }
            return ports.ToArray();
        }

        public override ServerProxy CreateServer()
        {
            return new JavaServer(ID, this);
        }
    }
}
