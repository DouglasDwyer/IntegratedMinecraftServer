using IMS_Interface.Preferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Preferences
{
    /// <summary>
    /// Represents the relationship between a <see cref="ServerPreferences"/> type and the display layout that goes with it.
    /// </summary>
    public sealed class ConfigurationPreferenceDisplayBinding
    {
        /// <summary>
        /// The <see cref="ServerPreferences"/> type that this instance refers to.
        /// </summary>
        public Type PreferenceType;
        /// <summary>
        /// The display layout to use when rendering the settings display for configurations of <see cref="PreferenceType"/>.
        /// </summary>
        public Func<List<PreferenceDisplay>> Layout;

        /// <summary>
        /// Creates a new <see cref="ConfigurationPreferenceDisplayBinding"/> instance with the specified type and generator of preference editor elements.
        /// </summary>
        /// <param name="preferenceType">The server configuration type that results in <paramref name="layout"/> being rendered.</param>
        /// <param name="layout">A function which generates the layout to render.</param>
        public ConfigurationPreferenceDisplayBinding(Type preferenceType, Func<List<PreferenceDisplay>> layout) {
            PreferenceType = preferenceType;
            Layout = layout;
        }
    }
}