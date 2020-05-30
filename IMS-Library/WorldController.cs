using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace IMS_Library
{
    public class WorldController
    {
        public IList<World> LoadedWorlds { get => Worlds.Values.ToList(); }
        protected Dictionary<Guid, World> Worlds = new Dictionary<Guid, World>();

        protected Timer DoUpdateTimer = new Timer();

        public void Start()
        {
            LoadWorldsFromDisk();
            DoUpdateTimer.Elapsed += (x, y) => IMS.AsThreadSafe(Update);
            DoUpdateTimer.Interval = 60 * 1000;
            DoUpdateTimer.Enabled = true;
        }

        protected void Update()
        {
            foreach(ServerProxy server in IMS.Instance.ServerManager.Servers)
            {
                Worlds[server.CurrentConfiguration.WorldID].RunBackupUpdates(server);
            }
        }

        public World GetWorldByID(Guid id)
        {
            return Worlds.ContainsKey(id) ? Worlds[id] : null;
        }

        protected void LoadWorldsFromDisk()
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

        public void AddWorldToRegistry(World world)
        {
            Worlds.Add(world.ID, world);
        }

        public async Task DeleteWorldAsync(World world)
        {
            if(!Worlds.Remove(world.ID))
            {
                throw new ArgumentException("World was not found in world registry.");
            }
            await Task.Run(() => Directory.Delete(world.FolderPath, true));
        }

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
