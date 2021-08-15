using IMS_Library.HTMLToMOTD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IMS_Library
{
    /// <summary>
    /// Contains settings information about a <see cref="JavaServer"/>.
    /// </summary>
    [Serializable]
    [XmlRoot(Namespace = "IMS_Service")]
    public class JavaServerConfiguration : ServerConfiguration
    {
        /// <summary>
        /// The version of Minecraft to run this server with, or null if the latest version should be used.
        /// </summary>
        public string ServerVersion = null;
        /// <summary>
        /// The minimum amount of memory (in megabytes) that the JVM should allocate.
        /// </summary>
        public int MinimumMemoryMB = 512;
        /// <summary>
        /// The maximum amout of memory (in megabytes) that the JVM should allocate.
        /// </summary>
        public int MaximumMemoryMB = 1024;
        /// <summary>
        /// Additional arguments that should be passed to the JVM when the server begins.
        /// </summary>
        public string JavaArguments = "-XX:+UnlockExperimentalVMOptions";

        /// <summary>
        /// Whether the server should kick non-creative players for flying.
        /// </summary>
        [ServerProperty("allow-flight")]
        public bool AllowFlight = false;
        /// <summary>
        /// Whether the server should allow people to enter the Nether.
        /// </summary>
        [ServerProperty("allow-nether")]
        public bool AllowNether = true;
        /// <summary>
        /// Whether interactions with the server console should appear in-game in server operators' chats.
        /// </summary>
        [ServerProperty("broadcast-console-to-ops")]
        public bool BroadcastConsoleToOps = true;
        /// <summary>
        /// Whether RCON interactions with the server console should appear in-game in server operators' chats.
        /// </summary>
        [ServerProperty("broadcast-rcon-to-ops")]
        public bool BroadcastRCONToOps = true;
        /// <summary>
        /// The difficulty of the Minecraft world.  0 = Peaceful, 1 = Easy, 2 = Normal, 3 = Hard.  This setting is overriden to "hard" when <see cref="HardcoreMode"/> is true.
        /// </summary>
        [ServerProperty("difficulty")]
        public int Difficulty = 1;
        /// <summary>
        /// Whether command blocks should run on the server.
        /// </summary>
        [ServerProperty("enable-command-block")]
        public bool EnableCommandBlocks = false;
        /// <summary>
        /// Whether the server should listen for RCON (remote console) requests.
        /// </summary>
        [ServerProperty("enable-rcon")]
        public bool EnableRCON = false;
        /// <summary>
        /// Whether the server should save chunks synchronously with the main game thread.
        /// </summary>
        [ServerProperty("sync-chunk-writes")]
        public bool SyncChunkWrites = true;
        /// <summary>
        /// Whether the server can be queried for information with the Game4Spy protocol.
        /// </summary>
        [ServerProperty("enable-query")]
        public bool EnableQuery = false;
        /// <summary>
        /// Whether the server should set players' gamemodes to default when they log in.
        /// </summary>
        [ServerProperty("force-gamemode")]
        public bool ForceGamemode = false;
        /// <summary>
        /// What operator level Minecraft command functions run at.
        /// </summary>
        [ServerProperty("function-permission-level")]
        public int FunctionPermissionLevel = 2;
        /// <summary>
        /// The default player gamemode.  0 = Survival, 1 = Creative, 2 = Adventure, 3 = Spectator
        /// </summary>
        [ServerProperty("gamemode")]
        public int Gamemode = 0;
        /// <summary>
        /// Whether the server should generate structures like villages or Nether fortresses.
        /// </summary>
        [ServerProperty("generate-structures")]
        public bool GenerateStructures = true;
        /// <summary>
        /// What settings the world generator should use when creating a new world.
        /// </summary>
        [ServerProperty("generator-settings")]
        public string GeneratorSettings = "";
        /// <summary>
        /// Whether hardcore mode is enabled (in hardcore mode, you cannot respawn after death - you only get one chance).
        /// </summary>
        [ServerProperty("hardcore")]
        public bool HardcoreMode = false;
        /// <summary>
        /// The name of the subfolder containing Minecraft world data.
        /// </summary>
        [ServerProperty("level-name")]
        public string LevelName = "world";
        /// <summary>
        /// The seed to use when generating a new world.
        /// </summary>
        [ServerProperty("level-seed")]
        public string LevelSeed = "";
        /// <summary>
        /// The type of level to use when generating a new world.
        /// </summary>
        [ServerProperty("level-type")]
        public string LevelType = "default";
        /// <summary>
        /// The maximum build height that players may place blocks at.  Terrain may still naturally generate above this height.
        /// </summary>
        [ServerProperty("max-build-height")]
        public int MaxBuildHeight = 256;
        /// <summary>
        /// The maximum number of players that may join the server at once.
        /// </summary>
        [ServerProperty("max-players")]
        public int MaxPlayers = 20;
        /// <summary>
        /// The maximum time the server watchdog thread should wait, in milliseconds, for a tick before it declares the server crashed.
        /// </summary>
        [ServerProperty("max-tick-time")]
        public int MaxTickTime = 60000;
        /// <summary>
        /// The distance from 0 to the world border.
        /// </summary>
        [ServerProperty("max-world-size")]
        public int MaxWorldSize = 29999984;
        /// <summary>
        /// The message of the day that displays on clients' screens.
        /// </summary>
        [MOTDServerProperty("motd")]
        public string MessageOfTheDay = "";
        /// <summary>
        /// The threshold (in bytes) at which the server should start compressing messages.
        /// </summary>
        [ServerProperty("network-compression-threshold")]
        public int NetworkCompressionThreshold = 256;
        /// <summary>
        /// Whether the server should verify the identities of connecting players by conferring with Mojang servers.
        /// </summary>
        [ServerProperty("online-mode")]
        public bool OnlineMode = true;
        /// <summary>
        /// The permission level given to operators when /op is used.
        /// </summary>
        [ServerProperty("op-permission-level")]
        public int DefaultOpPermissionLevel = 4;
        /// <summary>
        /// The amount of time to wait, in minutes, before kicking an idle player.  Setting this to 0 disables the behavior.
        /// </summary>
        [ServerProperty("player-idle-timeout")]
        public int PlayerIdleTimeout = 0;
        /// <summary>
        /// Whether the player should prevent connections from players using network proxies.
        /// </summary>
        [ServerProperty("prevent-proxy-connections")]
        public bool PreventProxyConnections = false;
        /// <summary>
        /// Whether players should be able to hit each other.
        /// </summary>
        [ServerProperty("pvp")]
        public bool EnablePVP = true;
        /// <summary>
        /// What port to listen on for Game4Spy queries.
        /// </summary>
        [ServerProperty("query.port")]
        public WebPort QueryPort = new WebPort(25565, true);
        /// <summary>
        /// The password that RCON applications should use to gain access to the server.
        /// </summary>
        [ServerProperty("rcon.password")]
        public string RCONPassword = "";
        /// <summary>
        /// The port that the server should listen on for RCON requests.
        /// </summary>
        [ServerProperty("rcon.port")]
        public WebPort RCONPort = new WebPort(25575, false);
        /// <summary>
        /// A URL to a resource pack that clients should download, or empty if a resource pack should not be used.
        /// </summary>
        [ServerProperty("resource-pack")]
        public string ResourcePack = "";
        /// <summary>
        /// The SHA-1 hash of the resource pack that should be used to check for the download's integrity, or empty if a check should not be employed.
        /// </summary>
        [ServerProperty("resource-pack-sha1")]
        public string ResourcePackSHA1 = "";
        /// <summary>
        /// The IP address that the server should bind to, or empty if the server should listen on all addresses.
        /// </summary>
        [ServerProperty("server-ip")]
        public string ServerIP = "";
        /// <summary>
        /// The port that the server should use to communicate with players.
        /// </summary>
        [ServerProperty("snooper-enabled")]
        public bool SnooperEnabled = true;
        /// <summary>
        /// Whether passive mobs (like cows) should spawn naturally.
        /// </summary>
        [ServerProperty("spawn-animals")]
        public bool SpawnAnimals = true;
        /// <summary>
        /// Whether monsters (like zombies) should spawn naturally.
        /// </summary>
        [ServerProperty("spawn-monsters")]
        public bool SpawnMonsters = true;
        /// <summary>
        /// Whether villagers should spawn naturally.
        /// </summary>
        [ServerProperty("spawn-npcs")]
        public bool SpawnVillagers = true;
        /// <summary>
        /// The amount of blocks from the world center that normal players should be prevented from editing.
        /// </summary>
        [ServerProperty("spawn-protection")]
        public int SpawnProtection = 0;
        /// <summary>
        /// Whether the server should use native network protocols on Linux machines.
        /// </summary>
        [ServerProperty("use-native-transport")]
        public bool UseLinuxNativeTransport = false;
        /// <summary>
        /// The distance (in chunks) that the server should render around any given player.
        /// </summary>
        [ServerProperty("view-distance")]
        public int ViewDistance = 10;
        /// <summary>
        /// Whether the player should only allow whitelisted players to jion.
        /// </summary>
        [ServerProperty("white-list")]
        public bool UseWhitelist = false;
        /// <summary>
        /// Whether the player should kick non-whitelisted players when the whitelist is enabled in-game.
        /// </summary>
        [ServerProperty("enforce-whitelist")]
        public bool EnforceWhitelist = false;

        /// <summary>
        /// Creates a new Java server settings object with a random unique identifier.
        /// </summary>
        public JavaServerConfiguration() {
            ServerPort = new WebPort(25565);
        }
        /// <summary>
        /// Creates a new Java server settings object with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier that this configuration should be associated with.</param>
        public JavaServerConfiguration(Guid id) : base(id) {
            ServerPort = new WebPort(25565);
        }

        /// <summary>
        /// Retrieves a list of all ports that the server is currently using.
        /// </summary>
        /// <returns>A list of ports.</returns>
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

        /// <summary>
        /// Retrieves a list of ports that should be forwarded by the server.
        /// </summary>
        /// <returns>A list of ports.</returns>
        public override int[] GetPortsToForward()
        {
            List<int> ports = new List<int>();
            if(ServerPort.AttemptUPnPForwarding)
            {
                ports.Add(ServerPort.Port);
            }
            if(EnableQuery && (ServerPort.Port != QueryPort.Port || !ServerPort.AttemptUPnPForwarding) && QueryPort.AttemptUPnPForwarding)
            {
                ports.Add(QueryPort.Port);
            }
            if(EnableRCON && RCONPort.AttemptUPnPForwarding)
            {
                ports.Add(RCONPort.Port);
            }
            return ports.ToArray();
        }

        /// <summary>
        /// Creates a <see cref="JavaServer"/> object that can be used to host a Minecraft server with the specified settings.
        /// </summary>
        /// <returns></returns>
        public override ServerProxy CreateServer()
        {
            return new JavaServer(ID, this);
        }
    }
}
