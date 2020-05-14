using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    [Serializable]
    public class MinecraftPlayer
    {
        public string Username;
        public string UUID;
        public string IP;
        public bool IsWhitelisted;
        public int PermissionLevel;
        public DateTime LastConnectionEvent;

        /*public static bool operator ==(MinecraftPlayer a, MinecraftPlayer b)
        {
            return
                a.Username == b.Username &&
                a.UUID == b.UUID &&
                a.IP == b.IP &&
                a.IsWhitelisted == b.IsWhitelisted &&
                a.PermissionLevel == b.PermissionLevel &&
                a.LastConnectionEvent == b.LastConnectionEvent;
        }

        public static bool operator !=(MinecraftPlayer a, MinecraftPlayer b)
        {
            return !(a == b);
        }*/
    }
}
