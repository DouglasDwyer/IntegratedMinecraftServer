using IMS_Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    [Serializable]
    public class BedrockServerConfiguration : ServerConfiguration
    {
        [ServerProperty("gamemode")]
        public int Gamemode = 0;
        [ServerProperty("difficulty")]
        public int Difficulty = 0;
        [ServerProperty("level-type")]
        public string LevelType = "DEFAULT";
        [ServerProperty("server-name")]
        public string ServerDisplayName = "Dedicated Server";
        [ServerProperty("max-players")]
        public int MaxPlayers = 10;
        [ServerProperty("server-port")]
        public WebPort ServerPort = new WebPort(19132);
        [ServerProperty("server-portv6")]
        public WebPort ServerPortV6 = new WebPort(19133);
        [ServerProperty("level-name")]
        public string LevelName = "level";
        [ServerProperty("level-seed")]
        public string LevelSeed = "";
        [ServerProperty("online-mode")]
        public bool OnlineMode = true;
        [ServerProperty("white-list")]
        public bool Whitelist = false;
        [ServerProperty("allow-cheats")]
        public bool AllowCheats = false;
        [ServerProperty("view-distance")]
        public int ViewDistance = 10;
        [ServerProperty("player-idle-timeout")]
        public int PlayerIdleTimeout = 30;
        [ServerProperty("max-threads")]
        public int MaximumThreads = 8;
        [ServerProperty("tick-distance")]
        public int TickDistance = 4;
        [ServerProperty("default-player-permission-level")]
        public string DefaultPlayerPermissionLevel = "member";
        [ServerProperty("texturepack-required")]
        public bool TexturePackRequired = false;

        public BedrockServerConfiguration() {
            Edition = MinecraftEdition.Bedrock;
        }
        public BedrockServerConfiguration(Guid id) : base(id)
        {
            Edition = MinecraftEdition.Bedrock;
        }

        public override int[] GetUsedPorts()
        {
            List<int> ports = new List<int>();
            ports.Add(ServerPort.Port);
            ports.Add(ServerPortV6.Port);
            return ports.ToArray();
        }

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

        public override ServerProxy CreateServer()
        {
            return new BedrockServer(ID, this);
        }
    }
}
