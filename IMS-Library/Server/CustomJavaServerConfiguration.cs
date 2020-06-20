using System;
using System.Collections.Generic;

namespace IMS_Library
{
    /// <summary>
    /// Represents the server settings for a Java server with a user-defined JAR.
    /// </summary>
    [Serializable]
    public class CustomJavaServerConfiguration : JavaServerConfiguration
    {
        /// <summary>
        /// A list of user-defined keys and values to be written to the server properties file.
        /// </summary>
        [ServerProperty(null)]
        public Dictionary<string, string> CustomServerProperties = new Dictionary<string, string>();

        /// <summary>
        /// Creates a <see cref="CustomJavaServer"/> object that can be used to host a Minecraft server with the specified settings.
        /// </summary>
        /// <returns>The server associated with these preferences.</returns>
        public override ServerProxy CreateServer()
        {
            return new CustomJavaServer(ID, this);
        }
    }
}