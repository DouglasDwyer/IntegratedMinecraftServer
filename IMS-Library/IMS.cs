using KinglyStudios.Knetworking;
using RoyalXML;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IMS_Library
{
    /// <summary>
    /// Represents the core Windows service which maintains Minecraft servers in the background.
    /// </summary>
    public sealed partial class IMS : ServiceBase
    {
        /// <summary>
        /// Returns the currently running instance of IMS.
        /// </summary>
        public static IMS Instance { get; private set; }

        /// <summary>
        /// Returns the currently active configuration for IMS.  To set the currently active configuration, use <see cref="ChangeSettings(IMSSettings)"/>.
        /// </summary>
        public IMSSettings CurrentSettings { get; private set; }
        /// <summary>
        /// Returns the current port manager, which can be used to forward/remove forwarded ports from UPnP routers.  Services are in charge of adding/removing their own ports.
        /// </summary>
        public PortForwarder PortManager { get; private set; }
        /// <summary>
        /// Returns the current firewall manager, which can be used to bypass the Windows firewall.  Services are in charge of adding/removing their own firewall exceptions.
        /// </summary>
        public FirewallController FirewallManager { get; private set; }
        /// <summary>
        /// Returns the current world manager, which is used to regulate the storage and backup of Minecraft worlds.
        /// </summary>
        public WorldController WorldManager { get; private set; }
        /// <summary>
        /// Returns the current web interface manager, which runs the administrator console for user configuration of IMS.
        /// </summary>
        public WebInterface WebServer { get; set; }
        /// <summary>
        /// Returns the current server manager, which regulates all loaded Minecraft server configurations.
        /// </summary>
        public ServerController ServerManager { get; private set; }
        /// <summary>
        /// Returns the current plugin manager, which regulates user-defined IMS plugins.
        /// </summary>
        public PluginController PluginManager { get; private set; }
        /// <summary>
        /// Returns the version provider, which can be used to fetch data about Minecraft server versions or download them off the internet.
        /// </summary>
        public MinecraftVersionProvider VersionManager { get; private set; }
        /// <summary>
        /// Returns the update manager, which is used to download/install the latest version of IMS from the internet.
        /// </summary>
        public UpdateController UpdateManager { get; private set; }

        private int exitCode = 0;

        public IMS()
        {
            ServiceName = "IMS";
            CanPauseAndContinue = false;
            CanShutdown = true;
            CanStop = true;
            Instance = this;
        }

        public void SimulateService()
        {
            OnStart(new string[0]);
            Console.ReadKey();
            OnStop();
        }

        /// <summary>
        /// Restarts the IMS Windows service.
        /// </summary>
        public void Restart()
        {
            
        }

        /// <summary>
        /// Stops the IMS Windows service, shutting down with the specified error code.
        /// </summary>
        /// <param name="error">The error code to return.</param>
        public void Stop(int error = 0)
        {
            exitCode = error;
            base.Stop();
        }

        private void Execute()
        {
            Logger.WriteInfo("Starting IMS...");

            CurrentSettings = new IMSSettings().FromConfiguration();
            Logger.WriteInfo("IMS settings configuration loaded.");
            Logger.WriteInfo("Attempting to find suitable UPnP router for port forwarding...");

            PluginManager = new PluginController();
            PluginManager.Initialize();

            FirewallManager = new FirewallController();
            FirewallManager.CreateFirewallExecutableException("MainService", Constants.ExecutionPath + "/IMS-Service.exe");

            PortManager = new PortForwarder();
            PortManager.Start();

            UpdateManager = new UpdateController();
            UpdateManager.Start();

            VersionManager = new MinecraftVersionProvider().FromConfiguration();
            VersionManager.Start();

            WorldManager = new WorldController();
            WorldManager.Start();

            ServerManager = new ServerController();
            ServerManager.Start();

            WebServer.Port = CurrentSettings.ManagementPort;
            WebServer.Start();

            PluginManager.Start();
        }

        protected override void OnStart(string[] args)
        {
            Execute();
        }

        protected override void OnStop()
        {
            PluginManager.Stop();
            WebServer.Stop();
            ServerManager.Stop();
            WorldManager.Stop();
            VersionManager.Stop();
            UpdateManager.Stop();
            PortManager.Stop();
            FirewallManager.RemoveFirewallExecutableException("MainService");
            CurrentSettings.SaveConfiguration();
            if (exitCode != 0)
            {
                Logger.FinishLog();
            }
            Environment.Exit(exitCode);
        }

        /// <summary>
        /// Updates <see cref="CurrentSettings"/>, making changes to other services (like forwarding ports) as necessary.
        /// </summary>
        /// <param name="newSettings">The settings to apply.</param>
        public void ChangeSettings(IMSSettings newSettings)
        {
            lock (CurrentSettings)
            {
                if (CurrentSettings.ManagementPort.AttemptUPnPForwarding)
                {
                    PortManager.RemovePort(CurrentSettings.ManagementPort.Port);
                }
                if (newSettings.ManagementPort.AttemptUPnPForwarding)
                {
                    PortManager.ForwardPort(newSettings.ManagementPort.Port);
                }
                if (CurrentSettings.ManagementPort.Port != newSettings.ManagementPort.Port)
                {
                    WebServer.Stop();
                    WebServer.Port = newSettings.ManagementPort;
                    WebServer.Start();
                }
                if (CurrentSettings.RunIMSOnStartup != newSettings.RunIMSOnStartup)
                {
                    if (newSettings.RunIMSOnStartup)
                    {
                        //set service to run on computer start
                    }
                    else
                    {
                        //set service to not run on computer start
                    }
                }
                CurrentSettings = newSettings;
                CurrentSettings.SaveConfiguration();
            }
        }
    }
}
