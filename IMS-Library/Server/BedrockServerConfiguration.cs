using IMS_Library;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace IMS_Library
{
    /// <summary>
    /// Contains settings information about a <see cref="BedrockServer"/>.
    /// </summary>
    [Serializable]
    public class BedrockServerConfiguration : ServerConfiguration
    {
        /// <summary>
        /// The default player gamemode.  0 = Survival, 1 = Creative, 2 = Adventure, 3 = Spectator
        /// </summary>
        [ServerProperty("gamemode")]
        public int Gamemode = 0;
        /// <summary>
        /// The difficulty of the Minecraft world.  0 = Peaceful, 1 = Easy, 2 = Normal, 3 = Hard
        /// </summary>
        [ServerProperty("difficulty")]
        public int Difficulty = 1;
        /// <summary>
        /// The type of level to use when generating a new world.
        /// </summary>
        [ServerProperty("level-type")]
        public string LevelType = "DEFAULT";
        /// <summary>
        /// The name of the server that should display on players' screens.
        /// </summary>
        [ServerProperty("server-name")]
        public string ServerDisplayName = "Dedicated Server";
        /// <summary>
        /// The maximum number of players that may join the server at once.
        /// </summary>
        [ServerProperty("max-players")]
        public int MaxPlayers = 10;
        /// <summary>
        /// The IPv6 port that the server should use to communicate with players.
        /// </summary>
        [ServerProperty("server-portv6")]
        public WebPort ServerPortV6 = new WebPort(19133);
        /// <summary>
        /// The name of the subfolder containing Minecraft world data.
        /// </summary>
        [ServerProperty("level-name")]
        public string LevelName = "level";
        /// <summary>
        /// The seed to use when generating a new world.
        /// </summary>
        [ServerProperty("level-seed")]
        public string LevelSeed = "";
        /// <summary>
        /// Whether the server should verify the identities of connecting players by conferring with Mojang servers.
        /// </summary>
        [ServerProperty("online-mode")]
        public bool OnlineMode = true;
        /// <summary>
        /// Whether the player should kick non-whitelisted players when the whitelist is enabled in-game.
        /// </summary>
        [ServerProperty("white-list")]
        public bool Whitelist = false;
        /// <summary>
        /// Whether server operators should be allowed to use commands in-game.
        /// </summary>
        [ServerProperty("allow-cheats")]
        public bool AllowCheats = false;
        /// <summary>
        /// The distance (in chunks) that the server should render around any given player.
        /// </summary>
        [ServerProperty("view-distance")]
        public int ViewDistance = 10;
        /// <summary>
        /// The amount of time to wait, in minutes, before kicking an idle player.  Setting this to 0 disables the behavior.
        /// </summary>
        [ServerProperty("player-idle-timeout")]
        public int PlayerIdleTimeout = 30;
        /// <summary>
        /// The maximum number of worker threads that the server should use for processing.
        /// </summary>
        [ServerProperty("max-threads")]
        public int MaximumThreads = 8;
        /// <summary>
        /// The maximum distance, in chunks, for ticking the world around any given player.
        /// </summary>
        [ServerProperty("tick-distance")]
        public int TickDistance = 4;
        /// <summary>
        /// The default permission level assigned to new players.
        /// </summary>
        [Obsolete]
        public string DefaultPlayerPermissionLevel = "member";
        /// <summary>
        /// The default permission level assigned to new players.
        /// </summary>
        [PlayerPermissionProperty("default-player-permission-level")]
        public PlayerPermissionLevel DefaultPlayerPermissionStatus = PlayerPermissionLevel.Member;
        /// <summary>
        /// Whether players must download a texture pack to play on the current server.
        /// </summary>
        [ServerProperty("texturepack-required")]
        public bool TexturePackRequired = false;
        /// <summary>
        /// Whether the server should log content errors to a file.
        /// </summary>
        [ServerProperty("content-log-file-enabled")]
        public bool ContentLogging = false;
        /// <summary>
        /// The threshold (in bytes) at which the server should start compressing messages.
        /// </summary>
        [ServerProperty("compression-threshold")]
        public int NetworkCompressionThreshold = 1;
        /// <summary>
        /// Whether the server should analyze client movement.  The server will only enforce correct movement if <see cref="CorrectPlayerMovement"/> is <c>true</c>.
        /// </summary>
        [Obsolete]
        public bool ServerAuthoritativeMovement = false;
        /// <summary>
        /// Whether the server should analyze client movement.  The server will only enforce correct movement if <see cref="CorrectPlayerMovement"/> is <c>true</c>.
        /// </summary>
        [AuthoritativeMovementProperty("server-authoritative-movement")]
        public AuthoritativeMovementType ServerAuthoritativeMovementConfiguration = AuthoritativeMovementType.Server;
        /// <summary>
        /// The number of times that abnormal behavior of a client can occur before it is reported.
        /// </summary>
        [ServerProperty("player-movement-score-threshold")]
        public int PlayerMovementScoreThreshold = 20;
        /// <summary>
        /// The difference between server and client positions that needs to be exceeded before abnormal behavior is detected.
        /// </summary>
        [ServerProperty("player-movement-distance-threshold")]
        public double PlayerMovementDistanceThreshold = 0.3;
        /// <summary>
        /// The duration of time, in milliseconds, the server and client positions can be out of sync (as defined by <see cref="PlayerMovementDistanceThreshold"/>) before the abnormal movement score is incremented.
        /// </summary>
        [ServerProperty("player-movement-duration-threshold-in-ms")]
        public int PlayerMovementDurationThreshold = 500;
        /// <summary>
        /// Whether the server should correct clients' position when their movement score exceeds <see cref="PlayerMovementScoreThreshold"/>.
        /// </summary>
        [ServerProperty("correct-player-movement")]
        public bool CorrectPlayerMovement = true;
        /// <summary>
        /// Whether the server should verify clients' attempts to break blocks.
        /// </summary>
        [ServerProperty("server-authoritative-block-breaking")]
        public bool ServerAuthoritativeBlockBreaking = false;

        /// <summary>
        /// Creates a new <see cref="BedrockServerConfiguration"/> instance with a new unique identifier.
        /// </summary>
        public BedrockServerConfiguration() {
            Edition = MinecraftEdition.Bedrock;
        }

        /// <summary>
        /// Creates a new <see cref="BedrockServerConfiguration"/> instance with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier to associate with this server configuration.</param>
        public BedrockServerConfiguration(Guid id) : base(id)
        {
            Edition = MinecraftEdition.Bedrock;
        }

        /// <summary>
        /// Retrieves a list of ports that this server uses.
        /// </summary>
        /// <returns>A list of port numbers.</returns>
        public override int[] GetUsedPorts()
        {
            List<int> ports = new List<int>();
            ports.Add(ServerPort.Port);
            ports.Add(ServerPortV6.Port);
            return ports.ToArray();
        }

        /// <summary>
        /// Retrieves a list of ports that this server uses which should be forwarded with a UPnP router.
        /// </summary>
        /// <returns>A list of port numbers.</returns>
        public override int[] GetPortsToForward()
        {
            List<int> ports = new List<int>();
            if (ServerPort.AttemptUPnPForwarding)
            {
                ports.Add(ServerPort.Port);
            }
            if (ServerPort.AttemptUPnPForwarding)
            {
                ports.Add(ServerPortV6.Port);
            }
            return ports.ToArray();
        }

        /// <summary>
        /// Creates a server object that can be used to run a Minecraft server with this configuration.
        /// </summary>
        /// <returns>A Minecraft server wrapper configured with these settings.</returns>
        public override ServerProxy CreateServer()
        {
            return new BedrockServer(ID, this);
        }

        /// <summary>
        /// The way in which a Bedrock server processes player movement.
        /// </summary>
        public enum AuthoritativeMovementType
        {
            /// <summary>
            /// Clients report their current position and movements to the server.
            /// </summary>
            Client = 0,
            /// <summary>
            /// Clients report user input to the server, which then computes their current position and movements.
            /// </summary>
            Server = 1,
            /// <summary>
            /// Clients report user input to the server, which then computes their current position and movements. The server may ask clients to repeat their inputs or recalculate when it determines they are out-of-sync.
            /// </summary>
            ServerWithRewind = 2
        }

        private class AuthoritativeMovementProperty : ServerProperty
        {
            public AuthoritativeMovementProperty(string propertyName) : base(propertyName)
            {
            }

            public override string GetData(ServerConfiguration configuration, FieldInfo field)
            {
                AuthoritativeMovementType movement = (AuthoritativeMovementType)field.GetValue(configuration);
                if(movement == AuthoritativeMovementType.Client)
                {
                    return PropertyName + "=" + "client-auth";
                }
                else if(movement == AuthoritativeMovementType.Server)
                {
                    return PropertyName + "=" + "server-auth";
                }
                else
                {
                    return PropertyName + "=" + "server-auth-with-rewind";
                }
            }
        }

        /// <summary>
        /// The permission level of a player on a Bedrock server, which determines what actions they can take.
        /// </summary>
        public enum PlayerPermissionLevel
        {
            /// <summary>
            /// The player interact with the environment.
            /// </summary>
            Visitor = 0,
            /// <summary>
            /// The player can interact with the environment and play the game normally, but cannot execute administrative commands.
            /// </summary>
            Member = 1,
            /// <summary>
            /// The player is an operator, and has access to administrative commands like kicking and banning.
            /// </summary>
            Operator = 2
        }

        private class PlayerPermissionProperty : ServerProperty
        {
            public PlayerPermissionProperty(string propertyName) : base(propertyName)
            {
            }

            public override string GetData(ServerConfiguration configuration, FieldInfo field)
            {
                return PropertyName + "=" + ((PlayerPermissionLevel)field.GetValue(configuration)).ToString().ToLowerInvariant();
            }
        }
    }
}
