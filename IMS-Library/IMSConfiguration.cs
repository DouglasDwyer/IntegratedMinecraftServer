using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using KinglyStudios.Knetworking;
using RoyalXML;

namespace IMS_Library
{
    /// <summary>
    /// This is a base class that can be used to easily store/serialize configuration files in the <see cref="RoyalXML"/> format.
    /// </summary>
    [Serializable]
    public class IMSConfiguration : ICloneable
    {
        /// <summary>
        /// Creates a new instance of <see cref="IMSConfiguration"/>.
        /// </summary>
        public IMSConfiguration() { }

        /// <summary>
        /// Creates a deep copy of the configuration.
        /// </summary>
        /// <returns>A new <see cref="IMSConfiguration"/> object that has fields with the same values, but different objects.</returns>
        public object Clone()
        {
            return SerializationManagement.ByteArrayToObject(SerializationManagement.ObjectToByteArray(this));
        }

        /// <summary>
        /// Retrieves the file path for the settings configuration file.
        /// </summary>
        /// <returns>The path of the configuration file.</returns>
        public virtual string GetDefaultFilePath()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// This is a helper class containing extension methods used for saving/loading <see cref="IMSConfiguration"/> objects.
    /// </summary>
    public static class IMSConfigurationUtility
    {
        /// <summary>
        /// Loads the configuration from its configuration file, or returns the present configuration if the file does not exist.
        /// </summary>
        /// <typeparam name="T">The <see cref="IMSConfiguration"/> type to load.</typeparam>
        /// <param name="imsConfiguration">The current configuration.</param>
        /// <returns>The newly-loaded configuration.</returns>
        public static T FromConfiguration<T>(this T imsConfiguration) where T : IMSConfiguration
        {
            if(File.Exists(imsConfiguration.GetDefaultFilePath()))
            {
                return RoyalSerializer.XMLToObject<T>(File.ReadAllText(imsConfiguration.GetDefaultFilePath()));
            }
            else
            {
                return imsConfiguration;
            }
        }

        /// <summary>
        /// Saves an <see cref="IMSConfiguration"/> to disk.
        /// </summary>
        /// <typeparam name="T">The <see cref="IMSConfiguration"/> type to save.</typeparam>
        /// <param name="imsConfiguration">The current configuration.</param>
        public static void SaveConfiguration<T>(this T imsConfiguration) where T : IMSConfiguration
        {
            lock(imsConfiguration)
            {
                string path = imsConfiguration.GetDefaultFilePath();
                string folder = Path.GetDirectoryName(path);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                File.WriteAllText(path, RoyalSerializer.ObjectToXML(imsConfiguration));
            }
        }
    }
}
