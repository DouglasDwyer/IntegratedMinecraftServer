using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;
using Open.Nat;
using System.Diagnostics;

namespace IMS_Library
{
    public class PortForwarder : IDisposable
    {
        public bool ConnectedToPortForwardableDevice => UPnPDevice != null;

        protected object Locker = new object();

        private NatDevice UPnPDevice;
        private Timer DeviceReconnectTimer;
        private List<int> Ports = new List<int>();
        
        public PortForwarder()
        {
            DeviceReconnectTimer = new Timer();
            DeviceReconnectTimer.Interval = Constants.CheckToEnsureNATConnectedInterval;
            DeviceReconnectTimer.Elapsed += CheckConnection;
            DeviceReconnectTimer.Start();
        }

        public void Start()
        {
            FindUPnPDevice();
        }

        protected void FindUPnPDevice()
        {
            lock (Locker)
            {
                NatDiscoverer discoverer = new NatDiscoverer();
                try
                {
                    UPnPDevice = null;
                    UPnPDevice = discoverer.DiscoverDeviceAsync(PortMapper.Upnp, new CancellationTokenSource(2000)).Result;
                    Logger.WriteInfo("Successfully connected to UPnP capable device with IP " + UPnPDevice.GetExternalIPAsync().Result);
                }
                catch (Exception)
                {
                    if (UPnPDevice is null)
                    {
                        Logger.WriteWarning("Failed to find UPnP capable device to port forward with.");
                    }
                }
            }
        }

        public void ForwardPort(int port)
        {
            lock (Locker)
            {
                SynchronizationContext context = SynchronizationContext.Current;
                SynchronizationContext.SetSynchronizationContext(null);
                Ports.Add(port);
                AttemptToForwardPortInternally(port);
                SynchronizationContext.SetSynchronizationContext(context);
            }
        }

        public void RemovePort(int port)
        {
            lock (Locker)
            {
                SynchronizationContext context = SynchronizationContext.Current;
                SynchronizationContext.SetSynchronizationContext(null);
                Ports.Remove(port);
                AttemptToRemovePortInternally(port);
                SynchronizationContext.SetSynchronizationContext(context);
            }
        }

        protected void AttemptToForwardPortInternally(int port)
        {
            try
            {
                UPnPDevice.CreatePortMapAsync(new Mapping(Protocol.Tcp, port, port, 30, ""));
                Logger.WriteInfo("Successfully forwarded port " + port);
            }
            catch(Exception e)
            {
                Logger.WriteWarning("Failed to forward port " + port + ".\n" + e);
            }
        }

        protected void AttemptToRemovePortInternally(int port)
        {
            try
            {
                UPnPDevice.DeletePortMapAsync(UPnPDevice.GetSpecificMappingAsync(Protocol.Tcp, port).Result).Wait();
                Logger.WriteInfo("Successfully removed forwarded port " + port);
            }
            catch(Exception e)
            {
                Logger.WriteWarning("Failed to remove forwarded port " + port + ".\n" + e);
            }
        }

        protected void CheckConnection(object sender, EventArgs args)
        {
            lock(Locker) {
                if (UPnPDevice is null)
                {
                    FindUPnPDevice();
                    if (UPnPDevice != null)
                    {
                        foreach (int port in Ports)
                        {
                            AttemptToForwardPortInternally(port);
                        }
                    }
                }
                else
                {
                    try
                    {
                        UPnPDevice.GetExternalIPAsync().Wait();
                    }
                    catch
                    {
                        Logger.WriteWarning("Lost connection to UPnP router.");
                        FindUPnPDevice();
                        if (UPnPDevice != null)
                        {
                            foreach (int port in Ports)
                            {
                                AttemptToForwardPortInternally(port);
                            }
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            lock (this)
            {
                DeviceReconnectTimer.Stop();
            }
        }
    }
}
