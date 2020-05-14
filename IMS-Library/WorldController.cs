using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IMS_Library
{
    public class WorldController
    {
        public IList<World> LoadedWorlds { get => Worlds.Values.ToList(); }
        protected Dictionary<Guid, World> Worlds = new Dictionary<Guid, World>();

        public void Start()
        {
            LoadWorldsFromDisk();
        }

        public World GetWorldByID(Guid id)
        {
            return Worlds[id];
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
                        catch (Exception e)
                        {
                            Logger.WriteError("Error loading world " + worldGuid + "!\n" + e);
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

        public void AddWorldToRegistry(World world)
        {
            Worlds.Add(world.ID, world);
        }
    }
}
