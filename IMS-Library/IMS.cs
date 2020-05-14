using KinglyStudios.Knetworking;
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

        private Thread MainThread;
        private int exitCode = 0;
        private ConcurrentQueue<Action> passedEvents = new ConcurrentQueue<Action>();

        public IMS()
        {
            ServiceName = "IMS";
            CanPauseAndContinue = false;
            CanShutdown = true;
            CanStop = true;

            Instance = this;
        }

        public EventHandler ThreadSafeEvent(Action action)
        {
            return (a, b) => { passedEvents.Enqueue(action); };
        }

        public EventHandler ThreadSafeEvent(Action<object, EventArgs> action)
        {
            return (a, b) => { passedEvents.Enqueue(() => { action(a, b); }); };
        }

        public DataReceivedEventHandler ThreadSafeDataEvent(Action<object, DataReceivedEventArgs> action)
        {
            return (a, b) => { passedEvents.Enqueue(() => { action(a, b); }); };
        }

        public static EventHandler AsThreadSafeEvent(Action action)
        {
            return Instance.ThreadSafeEvent(action);
        }

        public static EventHandler AsThreadSafeEvent(Action<object, EventArgs> action)
        {
            return Instance.ThreadSafeEvent(action);
        }

        public void ThreadSafe(Action action)
        {
            if (Thread.CurrentThread == MainThread)
            {
                action();
            }
            else
            {
                bool done = false;
                passedEvents.Enqueue(() => { action(); done = true; });
                while (!done) { Thread.Sleep(5); }
            }
        }

        public static void AsThreadSafe(Action action)
        {
            Instance.ThreadSafe(action);
        }

        public T ThreadSafe<T>(Func<T> action)
        {
            T returnValue = default;
            ThreadSafe(() => { returnValue = action(); });
            return returnValue;
        }

        public static T AsThreadSafe<T>(Func<T> action)
        {
            return Instance.ThreadSafe(action);
        }

        public static DataReceivedEventHandler AsThreadSafeDataEvent(Action<object, DataReceivedEventArgs> action)
        {
            return Instance.ThreadSafeDataEvent(action);
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

            PortManager = new PortForwarder();
            PortManager.Initialize();

            FirewallManager = new FirewallForwarder();

            WorldManager = new WorldController();
            WorldManager.Start();

            ServerManager = new ServerController();
            ServerManager.Start();

            WebServer.Port = CurrentSettings.ManagementPort;
            WebServer.Start();

            while(true)
            {
                Thread.Sleep(5);
                if(passedEvents.Count > 0)
                {
                    Action action = null;
                    while(passedEvents.TryDequeue(out action))
                    {
                        try
                        {
                            action();
                        }
                        catch(Exception e)
                        {
                            Logger.WriteError("An error occured during event processing:\n" + e);
                        }
                    }
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            MainThread = new Thread(Execute);
            MainThread.IsBackground = false;
            MainThread.Start();
        }

        protected override void OnStop()
        {
            IMS.AsThreadSafe(() =>
            {
                WebServer.Stop();
                ServerManager.Stop();
                PortManager.Dispose();
                CurrentSettings.SaveConfiguration();
                if(exitCode != 0)
                {
                    Logger.FinishLog();
                }
                Environment.Exit(exitCode);
            });
        }

        public void ChangeSettings(IMSSettings newSettings)
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
            if(CurrentSettings.RunIMSOnStartup != newSettings.RunIMSOnStartup)
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
        }
    }
}
