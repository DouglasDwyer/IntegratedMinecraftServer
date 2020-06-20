using System;

namespace IMS_Library
{
    /// <summary>
    /// Contains information about a plugin, such as the plugin's file path, name, and description.
    /// </summary>
    public class PluginInformation
    {
        /// <summary>
        /// The display name of the plugin.
        /// </summary>
        public string Name;
        /// <summary>
        /// The name of the plugin's creator.
        /// </summary>
        public string Author;
        /// <summary>
        /// A short description of the plugin.
        /// </summary>
        public string Description;
        /// <summary>
        /// The name of the plugin's assembly.
        /// </summary>
        public string AssemblyName;
        /// <summary>
        /// The name of the plugin's file.
        /// </summary>
        public string FileName;
        /// <summary>
        /// The version of the assembly.
        /// </summary>
        public Version AssemblyVersion;
        /// <summary>
        /// Whether the plugin is enabled and should be loaded by IMS.
        /// </summary>
        public bool Enabled = false;

        /// <summary>
        /// Creates a new instance of the plugin information class.
        /// </summary>
        public PluginInformation() { 
        }

        /// <summary>
        /// Creates a new instance of the plugin information class using the given plugin.
        /// </summary>
        /// <param name="plugin">The plugin to gather information about.</param>
        public PluginInformation(IMSPluginBase plugin)
        {
            Name = plugin.Name;
            Author = plugin.Author;
            Description = plugin.Description;
            AssemblyName = plugin.PluginAssemblyName;
            FileName = plugin.PluginAssembly.Location;
            AssemblyVersion = plugin.CurrentVersion;
            Enabled = true;
        }
    }
}