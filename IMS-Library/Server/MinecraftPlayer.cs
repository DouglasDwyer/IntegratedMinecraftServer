using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    /// <summary>
    /// Contains data about a player on a Minecraft server.
    /// </summary>
    [Serializable]
    public class MinecraftPlayer
    {
        /// <summary>
        /// The username/display name of the player.
        /// </summary>
        public string Username;
        /// <summary>
        /// The player's unique identifier.
        /// </summary>
        public string UUID;
        /// <summary>
        /// The last-recorded IP address of the player.
        /// </summary>
        public string IP;
        /// <summary>
        /// Whether the player has whitelist status on the server.
        /// </summary>
        public bool IsWhitelisted;
        /// <summary>
        /// The permission (op) level of the player.
        /// </summary>
        public int PermissionLevel;
        /// <summary>
        /// The last time that the player logged into or out of the server.
        /// </summary>
        public DateTime LastConnectionEvent;
    }
}
