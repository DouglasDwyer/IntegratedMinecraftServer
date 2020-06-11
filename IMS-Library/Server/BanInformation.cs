using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    /// <summary>
    /// Instances of this class contain information about a ban enacted on a Minecraft player.
    /// </summary>
    public class BanInformation
    {
        /// <summary>
        /// The player who was banned.
        /// </summary>
        public MinecraftPlayer Player;
        /// <summary>
        /// The time at which the player was banned.
        /// </summary>
        public string CreatedDate;
        /// <summary>
        /// The name of the person who banned the player.
        /// </summary>
        public string BanSource;
        /// <summary>
        /// The time at which the ban expires.
        /// </summary>
        public string ExpirationDate;
        /// <summary>
        /// The reason the player was banned.
        /// </summary>
        public string Reason;
    }
}
