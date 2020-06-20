using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    /// <summary>
    /// Represents a plugin which contains unique webpages.
    /// </summary>
    public interface IWebpageProvider
    {
        /// <summary>
        /// Returns a list of URLs, relative to the plugin's base URL, that map to Blazor component types.
        /// The plugin's base URL is "/Plugin/{PLUGIN ASSEMBLY NAME}/".  Relative URLs cannot include slashes.
        /// </summary>
        /// <returns>A dictionary containing URL-component mappings.</returns>
        public Dictionary<string, Type> GetPageRoutings();
    }
}
