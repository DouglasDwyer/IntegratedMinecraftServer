using System;
using System.Collections.Generic;
using System.Linq;
using IMS_Interface.Pages.Player;
using IMS_Interface.Pages.World;

namespace IMS_Interface
{
    public class Provider
    {
        public static readonly List<PlayerDisplay> PlayerManagerDisplays = new List<PlayerDisplay>();
        public static readonly List<WorldDisplay> WorldManagerDisplays = new List<WorldDisplay>();
        public static readonly List<NavigationBarLink> NavigationBarLinks = new List<NavigationBarLink>();
        public static readonly Dictionary<Type, BackupPolicyDisplay> BackupPolicyDisplayBinding = new Dictionary<Type, BackupPolicyDisplay>();
    }
}
