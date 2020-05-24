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
                return RoyalSerializer.XMLToObject<T>(File.ReadAllText(imsConfiguration.GetDefaultFilePath()));
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
                File.WriteAllText(path, RoyalSerializer.ObjectToXML(imsConfiguration));
            }
        }
    }
}
