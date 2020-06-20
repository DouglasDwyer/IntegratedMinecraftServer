using IMS_Interface.Preferences;
using IMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface
{
    /// <summary>
    /// Represents the relationship between a server type and a preference display layout for use in server creation.
    /// </summary>
    public class NewServerTypeBinding
    {
        /// <summary>
        /// The display name of the server type.
        /// </summary>
        public string DisplayName;
        /// <summary>
        /// A delegate which generates a preference layout for editing the server configuration defined in <see cref="PreferenceConfigurationGenerator"/>.
        /// </summary>
        public Func<List<PreferenceDisplay>> PreferenceLayoutGenerator;
        /// <summary>
        /// A delegate which creates a new <see cref="ServerConfiguration"/> for the specified server type.
        /// </summary>
        public Func<ServerConfiguration> PreferenceConfigurationGenerator;

        /// <summary>
        /// Creates a new instance of the <see cref="NewServerTypeBinding"/> class.
        /// </summary>
        public NewServerTypeBinding()
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="NewServerTypeBinding"/> class with the specified binding data.
        /// </summary>
        /// <param name="name">The display name of the server type.</param>
        /// <param name="configGenerator">A delegate which creates a new <see cref="ServerConfiguration"/> for the specified server type.</param>
        /// <param name="layoutGenerator">A delegate which generates a preference layout for editing the server configuration defined in <see cref="PreferenceConfigurationGenerator"/>.</param>
        public NewServerTypeBinding(string name, Func<ServerConfiguration> configGenerator, Func<List<PreferenceDisplay>> layoutGenerator)
        {
            DisplayName = name;
            PreferenceConfigurationGenerator = configGenerator;
            PreferenceLayoutGenerator = layoutGenerator;
        }
    }
}
