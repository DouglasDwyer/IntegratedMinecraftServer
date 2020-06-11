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
    /// <summary>
    /// This is a manager class used to regulate interactions with a UPnP router.  It can be used to automatically forward ports.
    /// </summary>
    public sealed class PortForwarder
    {
        /// <summary>
        /// This returns whether the <see cref="PortForwarder"/> object is currently connected to a UPnP capable router.
        /// </summary>
        public bool ConnectedToPortForwardableDevice => UPnPDevice != null;

        private object Locker = new object();

        private NatDevice UPnPDevice;
        private Timer DeviceReconnectTimer;
        private List<int> Ports = new List<int>();
        
        /// <summary>
        /// Creates a new <see cref="PortForwarder"/> instance.
        /// </summary>
        public PortForwarder()
        {
            DeviceReconnectTimer = new Timer();
            DeviceReconnectTimer.Interval = Constants.CheckToEnsureNATConnectedInterval;
            DeviceReconnectTimer.Elapsed += CheckConnection;
        }

        /// <summary>
        /// Begins the <see cref="PortForwarder"/> instance, attempting to find a UPnP capable router and starting network connection monitoring.
        /// </summary>
        public void Start()
        {
            FindUPnPDevice();
            DeviceReconnectTimer.Start();
        }

        private void FindUPnPDevice()
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

        /// <summary>
        /// Adds the specified port to a list of ports to forward, and attempts to forward the port.  If disconnected or not currently connected to a UPnP router, the port will be "remembered" and forwarded when possible.
        /// </summary>
        /// <param name="port">The port number to forward.</param>
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

        /// <summary>
        /// Removes the specified port from the port forwarding list.
        /// </summary>
        /// <param name="port">The port to remove.</param>
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

        private void AttemptToForwardPortInternally(int port)
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

        private void AttemptToRemovePortInternally(int port)
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

        private void CheckConnection(object sender, EventArgs args)
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

        /// <summary>
        /// Stops the port forwarding instance, removing all forwarded ports and stopping network connection monitoring.
        /// </summary>
        public void Stop()
        {
            lock (this)
            {
                foreach(int port in Ports)
                {
                    RemovePort(port);
                }
                DeviceReconnectTimer.Stop();
            }
        }
    }
}
