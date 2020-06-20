using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    /// <summary>
    /// Represents a collection of settings that affect IMS behavior.
    /// </summary>
    [Serializable]
    public class IMSSettings : IMSConfiguration
    {
        /// <summary>
        /// The port to run the IMS webserver on.
        /// </summary>
        public WebPort ManagementPort;
        /// <summary>
        /// Whether IMS should run when the computer boots.
        /// </summary>
        public bool RunIMSOnStartup = true;

        /// <summary>
        /// Determines the age that IMS logfiles must be for IMS to delete them.
        /// </summary>
        public TimeSpan LogDeletionTimespan = TimeSpan.FromHours(72);

        /// <summary>
        /// Represents the next time at which IMS should restart to install updates.  If null, IMS will never automatically install updates.
        /// </summary>
        public DateTime? IMSAutoUpdateTime = DateTime.Today.AddDays(1);

        /// <summary>
        /// The username to use when logging into the IMS admin console.
        /// </summary>
        public string Username;
        /// <summary>
        /// The SHA-256 hash of the password to use when logging into the IMS admin console.
        /// </summary>
        public byte[] PasswordHash;

        /// <summary>
        /// Creates a new instance of <see cref="IMSSettings"/>, with <see cref="ManagementPort"/> defaulting to 8080.
        /// </summary>
        public IMSSettings()
        {
            ManagementPort = new WebPort(8080, false);
        }

        /// <summary>
        /// Retrieves the file path for the settings configuration file.
        /// </summary>
        /// <returns>The path of the configuration file.</returns>
        public override string GetDefaultFilePath()
        {
            return Constants.ExecutionPath + Constants.ConfigurationFilePath;
        }
    }
}
