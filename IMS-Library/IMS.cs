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
    public partial class IMS : ServiceBase
    {
        public static IMS Instance { get; protected set; }

        public IMSSettings CurrentSettings { get; protected set; }
        public PortForwarder PortManager { get; protected set; }
        public FirewallForwarder FirewallManager { get; protected set; }
        public WorldController WorldManager { get; protected set; }
        public WebInterface WebServer { get; set; }
        public ServerController ServerManager { get; protected set; }
        public PluginController PluginManager { get; protected set; }
        public MinecraftVersionProvider VersionManager { get; protected set; }

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

        public void Restart()
        {
            
        }

        public void Stop(int error)
        {
            exitCode = error;
            Stop();
        }

        protected void Execute()
        {
            Logger.WriteInfo("Starting IMS...");

            CurrentSettings = new IMSSettings().FromConfiguration();
            Logger.WriteInfo("IMS settings configuration loaded.");
            Logger.WriteInfo("Attempting to find suitable UPnP router for port forwarding...");

            PluginManager = new PluginController();
            PluginManager.Initialize();

            PortManager = new PortForwarder();
            PortManager.Start();

            FirewallManager = new FirewallForwarder();

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
            PortManager.Dispose();
            CurrentSettings.SaveConfiguration();
            if (exitCode != 0)
            {
                Logger.FinishLog();
            }
            Environment.Exit(exitCode);
        }

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
