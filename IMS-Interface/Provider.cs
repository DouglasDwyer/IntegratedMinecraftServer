using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using IMS_Interface.Player;
using IMS_Interface.Preferences;
using IMS_Interface.World;

namespace IMS_Interface
{
    /// <summary>
    /// The <see cref="Provider"/> class provides various lists of displays to use when rendering parts of the IMS web interface.  It allows for plugin extensibility, as plugins can add their own displays to be rendered for many situations.
    /// </summary>
    public class Provider
    {
        public static readonly List<PlayerDisplay> PlayerManagerDisplays = new List<PlayerDisplay>();
        public static readonly List<WorldDisplay> WorldManagerDisplays = new List<WorldDisplay>();
        public static readonly List<NavigationBarLink> NavigationBarLinks = new List<NavigationBarLink>();
        public static readonly Dictionary<Type, BackupPolicyDisplay> BackupPolicyDisplayBinding = new Dictionary<Type, BackupPolicyDisplay>();
        /// <summary>
        /// This is a list which binds types to server preference displays.
        /// When <see cref="ServerPreferences"/> attempts to display server settings, it searches this list and chooses the binding whose <see cref="ConfigurationPreferenceDisplayBinding.PreferenceType"/> most directly matches the type of the settings to display.
        /// </summary>
        public static readonly ConcurrentBag<ConfigurationPreferenceDisplayBinding> ServerPreferenceDisplayBinding = new ConcurrentBag<ConfigurationPreferenceDisplayBinding>();
        /// <summary>
        /// This is a list which binds server types to "create new server" display views.
        /// </summary>
        public static readonly ConcurrentBag<NewServerTypeBinding> NewServerDisplayBinding = new ConcurrentBag<NewServerTypeBinding>();
    }
}
