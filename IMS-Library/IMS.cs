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
using System.Timers;
using System.Xml.Serialization;
using Timer = System.Timers.Timer;

namespace IMS_Library
{
    /// <summary>
    /// Represents the core Windows service which maintains Minecraft servers in the background.
    /// </summary>
    public sealed partial class IMS : ServiceBase, ILogProvider
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
        /// <summary>
        /// Returns the user message manager, which is used to send data messages to the IMS admin console.
        /// </summary>
        public InformationController UserMessageManager { get; set; }

        /// <summary>
        /// Returns whether IMS is running in developer mode - as a console application instead of a service.
        /// </summary>
        public bool IsDevelopmentMode { get; private set; } = false;

        private Timer SaveManagementTimer;
        private int exitCode = 0;
        /// <summary>
        /// Represents the path of an experimental plugin that IMS should load for use in plugin development.
        /// </summary>
        public string DevelopmentPluginPath = null;

        /// <summary>
        /// Creates a new instance of IMS.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if an instance of IMS already exists.
        /// </exception>
        public IMS()
        {
            if(Instance != null)
            {
                throw new InvalidOperationException("IMS is already running.");
            }
            ServiceName = "IMS";
            CanPauseAndContinue = false;
            CanShutdown = true;
            CanStop = true;
            Instance = this;
        }

        public void SimulateService()
        {
            IsDevelopmentMode = true;
            OnStart(new string[0]);
            Console.ReadKey();
            OnStop();
            Environment.Exit(0);
        }

        /// <summary>
        /// Restarts the IMS Windows service.
        /// </summary>
        public void Restart()
        {
            if (!IsDevelopmentMode)
            {
                Process.Start("cmd.exe", "/C net stop \"IMS\" && net start \"IMS\"");
            }
        }

        /// <summary>
        /// Stops the IMS Windows service, shutting down with the specified error code.
        /// </summary>
        /// <param name="error">The error code to return.</param>
        public void Stop(int error = 0)
        {
            if (!IsDevelopmentMode)
            {
                Process.Start("cmd.exe", "/C net stop \"IMS\"");
            }
        }

        private void Execute()
        {
            Logger.WriteInfo("Starting IMS...");

            CurrentSettings = new IMSSettings().FromConfiguration();
            Logger.WriteInfo("IMS settings configuration loaded.");
            Logger.WriteInfo("Attempting to find suitable UPnP router for port forwarding...");

            UserMessageManager = new InformationController().FromConfiguration();

            PluginManager = new PluginController().FromConfiguration();
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
            if(!string.IsNullOrEmpty(DevelopmentPluginPath))
            {
                try
                {
                    if (PluginManager.LoadedPlugins.Where(x => Path.GetFullPath(x.Value.PluginAssembly.Location) == Path.GetFullPath(DevelopmentPluginPath)).Count() == 0)
                    {
                        PluginManager.LoadPlugin(DevelopmentPluginPath);
                    }
                }
                catch(Exception e)
                {
                    UserMessageManager.LogError("Couldn't load development plugin!  See the console for more details.", false);
                    Logger.WriteError("Couldn't load development plugin at path '" + DevelopmentPluginPath + "'!  Error:\n" + e);
                }
            }

            StartLogDeletionTimer();
        }

        private void StartLogDeletionTimer()
        {
            SaveManagementTimer = new Timer();
            SaveManagementTimer.Interval = 15 * 60 * 1000;
            SaveManagementTimer.Elapsed += (x, y) => {
                ServerManager.SaveConfigurations();
                WorldManager.SaveConfigurations();
                UserMessageManager.SaveConfiguration();

                if (CurrentSettings.LogDeletionTimespan == default)
                {
                    return;
                }
                foreach (LogFileInformation info in GetAllLogFiles())
                {
                    if (info.CreationDate + CurrentSettings.LogDeletionTimespan < DateTime.Now)
                    {
                        DeleteLogFile(info);
                    }
                }
            };
            SaveManagementTimer.Start();
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
            UserMessageManager.SaveConfiguration();
            CurrentSettings.SaveConfiguration();
            if (exitCode == 0)
            {
                Logger.FinishLog();
            }
            else
            {
                Environment.Exit(exitCode);
            }
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
                        Process.Start("cmd.exe", "/C sc config IMS start=auto");
                    }
                    else
                    {
                        Process.Start("cmd.exe", "/C sc config IMS start=demand");
                    }
                }
                CurrentSettings = newSettings;
                CurrentSettings.SaveConfiguration();
            }
        }

        /// <summary>
        /// Retrieves all of the IMS logfiles that are currently stored.
        /// </summary>
        /// <returns>A list of logfiles.</returns>
        public IEnumerable<LogFileInformation> GetAllLogFiles()
        {
            List<LogFileInformation> toReturn = new List<LogFileInformation>();
            foreach(string file in Directory.GetFiles(Constants.ExecutionPath + Constants.LogLocation))
            {
                if(new FileInfo(file).FullName == new FileInfo(Logger.CurrentLogFile).FullName)
                {
                    continue;
                }
                LogFileInformation information = new LogFileInformation();
                information.Name = Path.GetFileNameWithoutExtension(file);
                information.CreationDate = File.GetCreationTime(file);
                string[] text = File.ReadAllLines(file);
                if(text.Length > 0)
                {
                    information.CleanExit = text[text.Length - 1] == "[INFO] Exited cleanly.";
                }
                toReturn.Add(information);
            }
            return toReturn;
        }

        /// <summary>
        /// Retrieves the content of a specific IMS logfile.
        /// </summary>
        /// <param name="information">The logfile to read.</param>
        /// <returns>The text contained within the logfile.</returns>
        public string GetLogFile(LogFileInformation information)
        {
            return File.ReadAllText(Constants.ExecutionPath + Constants.LogLocation + "/" + information.Name + ".txt");
        }

        /// <summary>
        /// Deletes the IMS logfile, removing it from disk.
        /// </summary>
        /// <param name="information">The logfile to delete.</param>
        public void DeleteLogFile(LogFileInformation information)
        {
            File.Delete(Constants.ExecutionPath + Constants.LogLocation + "/" + information.Name + ".txt");
        }
    }
}
