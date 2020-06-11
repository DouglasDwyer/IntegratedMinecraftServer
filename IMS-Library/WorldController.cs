using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Timers;

namespace IMS_Library
{
    /// <summary>
    /// Acts as a manager object which regulates the user's Minecraft worlds.
    /// </summary>
    public sealed class WorldController
    {
        /// <summary>
        /// A list which contains all currently loaded Minecraft worlds.
        /// </summary>
        public IList<World> LoadedWorlds { get => Worlds.Values.ToList(); }
        private ConcurrentDictionary<Guid, World> Worlds = new ConcurrentDictionary<Guid, World>();

        private Timer DoUpdateTimer = new Timer();

        /// <summary>
        /// Begins the <see cref="WorldController"/> instance, loading worlds from disk and starting the update timer for world backups.
        /// </summary>
        public void Start()
        {
            LoadWorldsFromDisk();
            DoUpdateTimer.Elapsed += (x, y) => Update();
            DoUpdateTimer.Interval = 60 * 1000;
            DoUpdateTimer.Enabled = true;
        }

        private void Update()
        {
            foreach (ServerProxy server in IMS.Instance.ServerManager.Servers)
            {
                Worlds[server.CurrentConfiguration.WorldID].RunBackupUpdates();
            }
        }

        /// <summary>
        /// Retrieves a <see cref="World"/> instance by its ID.
        /// </summary>
        /// <param name="id">The ID of the world to find.</param>
        /// <returns>The <see cref="World"/> object that was found, or if the ID was not found, null.</returns>
        public World GetWorldByID(Guid id)
        {
            return Worlds.ContainsKey(id) ? Worlds[id] : null;
        }

        private void LoadWorldsFromDisk()
        {
            if (!Directory.Exists(Constants.ExecutionPath + Constants.WorldFolderLocation))
            {
                Directory.CreateDirectory(Constants.ExecutionPath + Constants.WorldFolderLocation);
            }
            foreach (string folder in Directory.GetDirectories(Constants.ExecutionPath + Constants.WorldFolderLocation))
            {
                Guid worldGuid = Guid.Empty;
                if (Guid.TryParse(Path.GetFileName(folder), out worldGuid))
                {
                    if (File.Exists(folder + "/config.xml"))
                    {
                        try
                        {
                            Worlds[worldGuid] = new World(worldGuid).FromConfiguration();
                        }
                        catch (InvalidCastException e)
                        {
                            Logger.WriteError("Error loading world " + worldGuid + "!\n" + e);
                            throw;
                        }
                    }
                    else
                    {
                        Logger.WriteWarning("Couldn't find world configuration for folder " + folder);
                    }
                }
                else
                {
                    Logger.WriteWarning("Couldn't load folder " + folder + " as Minecraft world instance!");
                }
            }
        }

        /// <summary>
        /// Gets the server on which <paramref name="world"/> is currently running.
        /// </summary>
        /// <param name="world">The world whose server to obtain.</param>
        /// <returns>The server that is currently running <paramref name="world"/>, or null if no server is using it.</returns>
        public ServerProxy GetServerOfWorld(World world)
        {
            foreach(ServerProxy server in IMS.Instance.ServerManager.Servers)
            {
                if(server.CurrentConfiguration.WorldID == world.ID)
                {
                    return server;
                }
            }
            return null;
        }

        /// <summary>
        /// Adds a Minecraft world to the instance's world registry.
        /// </summary>
        /// <param name="world">The <see cref="World"/> to make known to IMS.</param>
        public void AddWorldToRegistry(World world)
        {
            Worlds[world.ID] = world;
            world.SaveConfiguration();
        }

        /// <summary>
        /// Deletes a world from the registry and removes it from disk.
        /// </summary>
        /// <param name="world">The world to delete.</param>
        /// <returns>A <see cref="Task"/> object which represents the progress of the world folder deletion.</returns>
        public async Task DeleteWorldAsync(World world)
        {
            if(!Worlds.Remove(world.ID))
            {
                throw new ArgumentException("World was not found in world registry.");
            }
            await Task.Run(() => Directory.Delete(world.FolderPath, true));
        }

        /// <summary>
        /// Stops the <see cref="WorldController"/> instance, saving worlds and disabling the backup timer.
        /// </summary>
        public void Stop()
        {
            DoUpdateTimer.Enabled = false;
            foreach(World world in Worlds.Values)
            {
                world.SaveConfiguration();
            }
        }
    }
}
