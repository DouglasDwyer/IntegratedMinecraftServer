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
        /// Adds a new server to the list of known servers.
        /// </summary>
        /// <param name="configuration">The configuration of the new server.</param>
        /// <returns>The newly created server.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <paramref name="configuration"/> does not have a unique identifier or if a server with the same ID already exists.
        /// </exception>
        public ServerProxy AddServer(ServerConfiguration configuration)
        {
            if (configuration.ID == default)
            {
                throw new InvalidOperationException("The server has not been assigned a proper unique identifier.");
            }
            if (LoadedServers.ContainsKey(configuration.ID))
            {
                throw new InvalidOperationException("There is already a server registered with that unique identifier.");
            }
            configuration.SaveConfiguration();
            ServerProxy toLoad = configuration.CreateServer();
            LoadedServers[configuration.ID] = toLoad;
            if (configuration.IsEnabled)
            {
                toLoad.StartAsync();
            }
            return toLoad;
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
                    loadedServer.StartAsync();
                }
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

        /// <summary>
        /// Deletes a server, removing it from the registry and deleting the associated files from disk.
        /// </summary>
        /// <param name="id">The unique identifier of the server to delete.</param>
        /// <returns>A <see cref="Task"/> object representing the current state of the removal operation.</returns>
        public async Task DeleteServerAsync(Guid id)
        {
            if(LoadedServers.TryRemove(id, out ServerProxy server))
            {
                await Task.Run(() => {
                    DeleteDirectoryAndJunctions(Constants.ExecutionPath + Constants.ServerFolderLocation + "/" + id);
                });
            }
        }

        private void DeleteDirectoryAndJunctions(string path)
        {
            foreach(string directory in Directory.GetDirectories(path))
            {
                if(JunctionPoint.Exists(directory))
                {
                    JunctionPoint.Delete(directory);
                }
                else
                {
                    DeleteDirectoryAndJunctions(directory);
                }
            }
            Directory.Delete(path, true);
        }

        /// <summary>
        /// Saves all server configurations to disk.
        /// </summary>
        public void SaveConfigurations()
        {
            foreach(ServerProxy server in LoadedServers.Values)
            {
                server.CurrentConfiguration.SaveConfiguration();
            }
        }
    }
}
