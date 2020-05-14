using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using KinglyStudios.Knetworking;

namespace IMS_Library
{
    [Serializable]
    public class IMSConfiguration : ICloneable
    {
        public IMSConfiguration() { }

        public object Clone()
        {
            return SerializationManagement.ByteArrayToObject(SerializationManagement.ObjectToByteArray(this));
        }

        public virtual string GetDefaultFilePath()
        {
            return "";
        }
    }

    public static class IMSConfigurationUtility
    {
        public static T FromConfiguration<T>(this T imsConfiguration) where T : IMSConfiguration
        {
            if(File.Exists(imsConfiguration.GetDefaultFilePath()))
            {
                string config = File.ReadAllText(imsConfiguration.GetDefaultFilePath());
                return (T)SerializationManagement.XMLToObject(config.Substring(config.IndexOf("\n") + 1), Type.GetType(config.Remove(config.IndexOf("\n"))));
            }
            else
            {
                return imsConfiguration;
            }
        }

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
                File.WriteAllText(path, imsConfiguration.GetType().AssemblyQualifiedName + "\n" + SerializationManagement.ObjectToXML(imsConfiguration));
            }
        }

        private static string ServerConfigurationToXML(ServerConfiguration dataToSerialize)
        {
            using (StringWriter stringwriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(ServerConfiguration));
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
        }

        private static ServerConfiguration XMLToServerConfiguration(string xmlText)
        {
            using (StringReader stringReader = new StringReader(xmlText))
            {
                return (ServerConfiguration)new XmlSerializer(typeof(ServerConfiguration)).Deserialize(stringReader);
            }
        }
    }

}
