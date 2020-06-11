using KinglyStudios.Knetworking;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMS_Library
{
    /// <summary>
    /// Acts as a manager for the user's Minecraft servers, loading them from disk, starting them, and forwarding ports appropriately.
    /// </summary>
    public sealed class ServerController
    {
        /// <summary>
        /// A list containing all currently loaded servers.
        /// </summary>
        public IList<ServerProxy> Servers { get { return LoadedServers.Values.ToList().AsReadOnly(); } }
        private ConcurrentDictionary<Guid, ServerProxy> LoadedServers = new ConcurrentDictionary<Guid, ServerProxy>();

        private List<int> UsedPorts = new List<int>();

        /// <summary>
        /// Begins the <see cref="ServerController"/> instance, loading and starting Minecraft servers.
        /// </summary>
        public void Start()
        {
            Logger.WriteInfo("Loading servers from disk...");
            LoadServersFromDisk();
            Logger.WriteInfo("Servers loaded.  Starting servers...");
            StartAllEnabledServers();
            Logger.WriteInfo("All servers started.");
        }

        /// <summary>
        /// Stops the <see cref="ServerController"/> instance, stopping and saving all servers.
        /// </summary>
        public void Stop()
        {
            Logger.WriteInfo("Shutting down all servers...");
            foreach(ServerProxy server in LoadedServers.Values)
            {
                if (server.State != ServerProxy.ServerState.Disabled)
                {
                    server.StopAsync().Wait();
                    RemoveForwardedPorts(server.CurrentConfiguration.GetPortsToForward());
                }
                server.CurrentConfiguration.SaveConfiguration();
            }
            Logger.WriteInfo("All servers shut down.");
        }

        private void StartAllEnabledServers()
        {
            foreach(ServerProxy loadedServer in LoadedServers.Values)
            {
                ServerConfiguration configuration = loadedServer.CurrentConfiguration;
                if (configuration.IsEnabled)
                {
                    int[] ports = configuration.GetUsedPorts();
                    foreach(int port in ports)
                    {
                        if(UsedPorts.Contains(port))
                        {
                            Logger.WriteError("Couldn't start server " + configuration.ServerName + " as it attempts to use port " + port + " which is already in use.");
                            configuration.IsEnabled = false;
                        }
                    }
                    if (configuration.IsEnabled)
                    {
                        UsedPorts.AddRange(ports);
                        ForwardPorts(configuration.GetPortsToForward());
                        loadedServer.StartAsync();
                    }
                }
            }
        }

        private void ForwardPorts(int[] ports)
        {
            foreach(int port in ports)
            {
                IMS.Instance.PortManager.ForwardPort(port);
            }
        }

        private void RemoveForwardedPorts(int[] ports)
        {
            foreach (int port in ports)
            {
                IMS.Instance.PortManager.RemovePort(port);
            }
        }

        private void LoadServersFromDisk()
        {
            if(!Directory.Exists(Constants.ExecutionPath + Constants.ServerFolderLocation))
            {
                Directory.CreateDirectory(Constants.ExecutionPath + Constants.ServerFolderLocation);
            }
            foreach(string folder in Directory.GetDirectories(Constants.ExecutionPath + Constants.ServerFolderLocation))
            {
                Guid serverGuid = Guid.Empty;
                if(Guid.TryParse(Path.GetFileName(folder), out serverGuid))
                {
                    if(File.Exists(folder + "/config.xml"))
                    {
                        try
                        {
                            ServerConfiguration config = new ServerConfiguration(serverGuid).FromConfiguration();
                            ServerProxy toAdd = config.CreateServer();
                            LoadedServers[serverGuid] = toAdd;
                        }
                        catch(Exception e)
                        {
                            Logger.WriteError("Error loading server " + serverGuid + "!\n" + e);
                        }
                    }
                    else
                    {
                        Logger.WriteWarning("Couldn't find server configuration for folder " + folder);
                    }
                }
                else
                {
                    Logger.WriteWarning("Couldn't load folder " + folder + " as Minecraft server instance!");
                }
            }
        }

        /// <summary>
        /// Retrieves a server by its ID.
        /// </summary>
        /// <param name="server">The ID of the server to retrieve.</param>
        /// <returns>The <see cref="ServerProxy"/> associated with this ID, or null if no server is found.</returns>
        public ServerProxy GetServer(Guid server)
        {
            return LoadedServers.ContainsKey(server) ? LoadedServers[server] : null;
        }
    }
}
