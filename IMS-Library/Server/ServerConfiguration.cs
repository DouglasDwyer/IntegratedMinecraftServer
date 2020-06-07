using System;
using System.IO;
using System.Xml.Serialization;

namespace IMS_Library
{
    [Serializable]
    public class ServerConfiguration : IMSConfiguration
    {
        public Guid ID;
        public string ServerName;
        public MinecraftEdition Edition;
        public bool IsEnabled;
        public Guid WorldID;

        public ServerConfiguration() { }

        public ServerConfiguration(Guid id)
        {
            ID = id;
        }

        public string GetServerFolderLocation()
        {
            return Constants.ExecutionPath + Constants.ServerFolderLocation + "/" + ID;
        }

        public override string GetDefaultFilePath()
        {
            return GetServerFolderLocation() + "/config.xml";
        }

        public virtual int[] GetUsedPorts()
        {
            return new int[0];
        }

        public virtual int[] GetPortsToForward()
        {
            return new int[0];
        }

        public virtual ServerProxy CreateServer()
        {
            throw new NotImplementedException();
        }
    }
}