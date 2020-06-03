using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Runtime.Loader;

namespace IMS_Library
{
    public sealed class PluginController
    {
        public IList<IMSPluginBase> LoadedPlugins { get => Plugins.AsReadOnly(); }
        private List<IMSPluginBase> Plugins = new List<IMSPluginBase>();

        protected object Locker = new object();

        public void Initialize()
        {
            lock (Locker)
            foreach (string plugin in IMS.Instance.CurrentSettings.PluginPaths)
            {
                try
                {
                    if (File.Exists(plugin))
                    {
                        LoadPlugin(plugin);
                    }
                }
                catch(Exception e)
                {
                    Logger.WriteError("Couldn't pre-load plugin assembly " + plugin + "!\n" + e);
                }
            }
        }

        public void Start()
        {
            lock(Locker)
            foreach(IMSPluginBase plugin in Plugins)
            {
                plugin.Start();
            }
        }

        public void UnloadPlugin(IMSPluginBase plugin)
        {
            lock (Locker)
            if (Plugins.Contains(plugin))
            {
                plugin.Stop();
                Plugins.Remove(plugin);
            }
            else
            {
                throw new ArgumentException("This plugin object has not been loaded by the plugin manager.");
            }
        }

        public void LoadPlugin(string path)
        {
            lock (Locker)
            {
                AssemblyLoadContext context = new AssemblyLoadContext(null, true);
                Assembly pluginAssembly = context.LoadFromAssemblyPath(path);
                try
                {
                    Plugins.Add((IMSPluginBase)Activator.CreateInstance(pluginAssembly.GetTypes().Where(x => x.BaseType == typeof(IMSPluginBase)).Single()));
                }
                finally
                {
                    context.Unload(); //start context unload now so the assembly disappears once the plugin is gc'd
                }
            }
        }

        public void Stop()
        {
            lock(Locker)
            foreach(IMSPluginBase plugin in Plugins.ToArray())
            {
                UnloadPlugin(plugin);
            }
        }
    }
}
