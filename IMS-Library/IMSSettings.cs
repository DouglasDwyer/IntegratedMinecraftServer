using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    [Serializable]
    public class IMSSettings : IMSConfiguration
    {
        public WebPort ManagementPort;
        public bool RunIMSOnStartup = true;

        public List<string> PluginPaths = new List<string>();

        public string Username;
        public byte[] PasswordHash;

        public IMSSettings()
        {
            ManagementPort = new WebPort(8080, false);
        }

        public override string GetDefaultFilePath()
        {
            return Constants.ExecutionPath + Constants.ConfigurationFilePath;
        }
    }
}
