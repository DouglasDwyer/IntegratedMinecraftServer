using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Xml.Serialization;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace IMS_Library
{
    /// <summary>
    /// Controls the management of third-party plugins.
    /// </summary>
    public sealed class PluginController : IMSConfiguration
    {
        /// <summary>
        /// All of the currently loaded plugins.
        /// </summary>
        public IReadOnlyDictionary<string, IMSPluginBase> LoadedPlugins => (IReadOnlyDictionary<string, IMSPluginBase>)Plugins.ToDictionary(pair => pair.Key, pair => pair.Value);
        private Dictionary<string, IMSPluginBase> Plugins = new Dictionary<string, IMSPluginBase>();

        /// <summary>
        /// This list contains all plugins that IMS is currently aware of, including unloaded plugins, indexed by assembly name.
        /// </summary>
        public Dictionary<string, PluginInformation> KnownPlugins = new Dictionary<string, PluginInformation>();

        private object Locker = new object();
        private string PluginPath => Constants.ExecutionPath + Constants.PluginFolderLocation;

        /// <summary>
        /// Loads all plugin assemblies into the current .NET runtime, but does not call <see cref="IMSPluginBase.Start"/>.
        /// </summary>
        public void Initialize()
        {
            lock (Locker) {
                AppDomain.CurrentDomain.UnhandledException += PreventPluginCrash;
                foreach (PluginInformation plugin in KnownPlugins.Values.ToArray())
                {
                    try
                    {
                        if (plugin.Enabled)
                        {
                            LoadPluginAssembly(plugin.FileName);
                        }
                    }
                    catch (Exception e)
                    {
                        KnownPlugins.Remove(plugin.AssemblyName);
                        Logger.WriteError("Couldn't pre-load plugin assembly " + plugin + "!\n" + e);
                    }
                }
            }
        }

        //This attempts to prevent the program from crashing if a plugin throws an exception.
        //It is a rather dirty approach and repeated calls will cause a memory leak.
        private void PreventPluginCrash(object sender, UnhandledExceptionEventArgs e)
        {
            StackTrace trace = new StackTrace((Exception)e.ExceptionObject);
            foreach(StackFrame frame in trace.GetFrames())
            {
                Assembly frameAssembly = frame.GetMethod().DeclaringType.Assembly;
                if(Plugins.ContainsKey(frameAssembly.GetName().Name))
                {
                    IMSPluginBase plugin = Plugins[frameAssembly.GetName().Name];
                    Logger.WriteError("An uncaught exception was raised in plugin " + plugin.Name + "!  The plugin has been disabled.  Error:\n" + e.ExceptionObject);
                    try
                    {
                        plugin.Stop();
                    }
                    catch(Exception stopException)
                    {
                        Logger.WriteError("During fatal error unload, the plugin's stop method also failed.  Exception:\n" + stopException);
                    }
                    Plugins.Remove(plugin.Name);
                    KnownPlugins[plugin.PluginAssembly.GetName().Name].Enabled = false;
                    Thread.CurrentThread.IsBackground = true;
                    Thread.CurrentThread.Join(); //suspend the thread indefinitely to stop the CLR from shutting down
                }
            }
            Logger.WriteError("A fatal error occured, resulting in IMS shutting down!\n" + e.ExceptionObject);
        }

        /// <summary>
        /// Calls <see cref="IMSPluginBase.Start"/> on each loaded plugin.
        /// </summary>
        public void Start()
        {
            lock(Locker)
            foreach(IMSPluginBase plugin in Plugins.Values)
            {
                plugin.Start();
            }
        }

        /// <summary>
        /// Attempts to remove a plugin from memory.  This does not guarantee that the plugin's assembly will be unloaded.
        /// </summary>
        /// <param name="plugin">The plugin to unload.</param>
        public void UnloadPlugin(IMSPluginBase plugin)
        {
            lock (Locker)
            if (Plugins.ContainsKey(plugin.PluginAssemblyName))
            {
                plugin.Stop();
                Plugins.Remove(plugin.PluginAssemblyName);
            }
            else
            {
                throw new ArgumentException("This plugin object has not been loaded by the plugin manager.");
            }
        }

        /// <summary>
        /// Attempts to remove a plugin from memory.  This does not guarantee that the plugin's assembly will be unloaded.
        /// </summary>
        /// <param name="name">The assembly name of the plugin to unload.</param>
        public void UnloadPlugin(string name)
        {
            lock (Locker)
                if (Plugins.ContainsKey(name))
                {
                    Plugins[name].Stop();
                    Plugins.Remove(name);
                }
                else
                {
                    throw new ArgumentException("This plugin object has not been loaded by the plugin manager.");
                }
        }

        /// <summary>
        /// Attempts to load a plugin into memory from the specified info.
        /// </summary>
        /// <param name="info">The plugin to load.</param>
        /// <returns>The loaded plugin.</returns>
        public IMSPluginBase LoadPlugin(PluginInformation info)
        {
            return LoadPlugin(info.FileName);
        }

        /// <summary>
        /// Attempts to load a plugin into memory from the specified path.
        /// </summary>
        /// <param name="path">The path of the plugin to load.</param>
        /// <returns>The loaded plugin.</returns>
        public IMSPluginBase LoadPlugin(string path)
        {
            IMSPluginBase plugin = LoadPluginAssembly(path);
            plugin.Start();
            return plugin;
        }

        /// <summary>
        /// Deletes the specified plugin's information from the registry.
        /// </summary>
        /// <param name="information">The plugin to delete.</param>
        public void DeletePlugin(PluginInformation information)
        {
            lock(Locker)
            {
                if(information.Enabled)
                {
                    UnloadPlugin(information.AssemblyName);
                }
                KnownPlugins.Remove(information.AssemblyName);
            }
        }

        private IMSPluginBase LoadPluginAssembly(string path)
        {
            lock (Locker)
            {
                AssemblyLoadContext context = new AssemblyLoadContext(null, true);
                Assembly pluginAssembly = context.LoadFromAssemblyPath(path);
                try
                {
                    IMSPluginBase plugin = (IMSPluginBase)Activator.CreateInstance(pluginAssembly.GetTypes().Where(x => x.BaseType == typeof(IMSPluginBase)).Single());
                    Plugins[plugin.PluginAssemblyName] = plugin;
                    KnownPlugins[plugin.PluginAssemblyName] = new PluginInformation(plugin);
                    return plugin;
                }
                finally
                {
                    context.Unload(); //start context unload now so the assembly disappears once the plugin is gc'd
                }
            }
        }

        /// <summary>
        /// Stops all plugins, unloading them.
        /// </summary>
        public void Stop()
        {
            lock (Locker)
            {
                foreach (IMSPluginBase plugin in Plugins.Values)
                {
                    UnloadPlugin(plugin);
                }
                AppDomain.CurrentDomain.UnhandledException -= PreventPluginCrash;
            }
            this.SaveConfiguration();
        }

        /// <summary>
        /// Retrieves the location of the plugin controller's settings file.
        /// </summary>
        /// <returns>The absolute path of the configuration file.</returns>
        public override string GetDefaultFilePath()
        {
            return PluginPath + "/plugins.xml";
        }
    }
}
