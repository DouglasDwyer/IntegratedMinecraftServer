using System;
using System.IO;
using System.Xml.Serialization;

namespace IMS_Library
{
    /// <summary>
    /// Represents the current settings of a <see cref="ServerProxy"/>.
    /// </summary>
    [Serializable]
    public class ServerConfiguration : IMSConfiguration
    {
        /// <summary>
        /// The unique identifier of the server.
        /// </summary>
        public Guid ID;
        /// <summary>
        /// The display name of the server.
        /// </summary>
        public string ServerName;
        /// <summary>
        /// The edition of Minecraft that this server supports.
        /// </summary>
        public MinecraftEdition Edition;
        /// <summary>
        /// Whether the server should be run by IMS, or if it should remain disabled.
        /// </summary>
        public bool IsEnabled;
        /// <summary>
        /// The unique identifier of the <see cref="World"/> that this server is currently using.
        /// </summary>
        public Guid WorldID;

        /// <summary>
        /// Creates a new <see cref="ServerConfiguration"/> instance with a new unique identifier.
        /// </summary>
        public ServerConfiguration() {
            ID = Guid.NewGuid();
        }

        /// <summary>
        /// Creates a new <see cref="ServerConfiguration"/> instance with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the server to use.</param>
        public ServerConfiguration(Guid id)
        {
            ID = id;
        }

        /// <summary>
        /// Retrieves the location of the Minecraft server's folder.
        /// </summary>
        /// <returns>The absolute path to the folder on disk.</returns>
        public string GetServerFolderLocation()
        {
            return Constants.ExecutionPath + Constants.ServerFolderLocation + "/" + ID;
        }

        /// <summary>
        /// Retrieves the location of the server's configuration file.
        /// </summary>
        /// <returns>The absolute path of the server's configuration file on disk.</returns>
        public override string GetDefaultFilePath()
        {
            return GetServerFolderLocation() + "/config.xml";
        }

        /// <summary>
        /// Retrieves a list of network ports that the server uses.
        /// </summary>
        /// <returns>A list of port numbers.</returns>
        public virtual int[] GetUsedPorts()
        {
            return new int[0];
        }

        /// <summary>
        /// Retrieves a list of network ports that should be forwarded with a UPnP router.
        /// </summary>
        /// <returns>A list of port numbers.</returns>
        public virtual int[] GetPortsToForward()
        {
            return new int[0];
        }

        /// <summary>
        /// Creates a <see cref="ServerProxy"/> object that can be used to control the server.
        /// </summary>
        /// <returns>A new server controller object.</returns>
        public virtual ServerProxy CreateServer()
        {
            throw new NotImplementedException();
        }
    }
}