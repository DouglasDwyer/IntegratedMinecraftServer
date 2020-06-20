using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    /// <summary>
    /// Represents a Java server that runs on a user-defined JAR file.
    /// </summary>
    public class CustomJavaServer : JavaServer
    {
        /// <summary>
        /// Creates a new custom Java server using the specified settings.
        /// </summary>
        /// <param name="id">The unique identifier to associate this server with.</param>
        /// <param name="config">The server settings to use.</param>
        public CustomJavaServer(Guid id, CustomJavaServerConfiguration config) : base(id, config) { }

        /// <summary>
        /// The location of the server JAR file that should be executed.
        /// </summary>
        protected override string JarLocation => Constants.ExecutionPath + Constants.ServerFolderLocation + "/" + ID + "/server.jar";

        /// <summary>
        /// Starts the internal Minecraft server process.  This call does not complete until the server leaves the <see cref="ServerProxy.ServerState.Starting"/> state.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents the current start operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the server is already running when this method is called, or if no JAR file exists for the server to run.
        /// </exception>
        public override Task StartAsync()
        {
            if(!File.Exists(JarLocation))
            {
                throw new InvalidOperationException("There is no JAR file associated with this server!  As such, the server cannot start.");
            }
            return base.StartAsync();
        }

        /// <summary>
        /// Returns a string that describes the current object.
        /// </summary>
        /// <returns>A string with the name of this server type.</returns>
        public override string ToString()
        {
            return "Java (custom)";
        }
    }
}
