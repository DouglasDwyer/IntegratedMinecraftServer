using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace IMS_Library
{
    /// <summary>
    /// Represents a plugin that adds functionality to IMS.
    /// </summary>
    public abstract class IMSPluginBase
    {
        /// <summary>
        /// The instance of IMS that is currently running.
        /// </summary>
        public IMS Service { get => IMS.Instance; }

        /// <summary>
        /// The assembly that contains this plugin.  An IMS plugin assembly may only contain one plugin.
        /// </summary>
        public Assembly PluginAssembly => GetType().Assembly;
        /// <summary>
        /// The name of the assembly that contains this plugin.  An IMS plugin assembly may only contain one plugin.
        /// </summary>
        public string PluginAssemblyName => PluginAssembly.GetName().Name;
        /// <summary>
        /// The human-readable name of this plugin.
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// The name of the plugin's creator.
        /// </summary>
        public abstract string Author { get; }
        /// <summary>
        /// A description of what this plugin does.
        /// </summary>
        public abstract string Description { get; }
        /// <summary>
        /// The version of the plugin's assembly.
        /// </summary>
        public Version CurrentVersion => PluginAssembly.GetName().Version;

        /// <summary>
        /// This method is called when the plugin is first loaded.
        /// It provides the ability for the plugin to register with any necessary controllers,
        /// or do things like set up timers.
        /// </summary>
        public virtual void Start() { }
        /// <summary>
        /// This method is called when the plugin is unloaded.
        /// The plugin should stop, unregister with any controllers it was using
        /// (such as removing forwarded ports), and prepare to be unloaded.
        /// </summary>
        public virtual void Stop() { }
    }
}
