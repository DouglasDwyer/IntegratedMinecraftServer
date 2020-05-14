using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Timer = System.Timers.Timer;
using Open.Nat;

namespace IMS_Library
{
    public class PortForwarder : IDisposable
    {
        private NatDevice upnpDevice;
        private bool failedToFindCapableDevice = false;
        private Timer deviceReconnectTimer;
        private List<int> ports = new List<int>();
        
        public PortForwarder()
        {
            deviceReconnectTimer = new Timer();
            deviceReconnectTimer.Interval = Constants.CheckToEnsureNATConnectedInterval;
            deviceReconnectTimer.Elapsed += CheckConnection;
            deviceReconnectTimer.Start();
        }

        public void Initialize()
        {
            FindUPnPDevice();
        }

        protected void FindUPnPDevice()
        {
            lock (this)
            {
                NatDiscoverer discoverer = new NatDiscoverer();
                try
                {
                    upnpDevice = discoverer.DiscoverDeviceAsync(PortMapper.Upnp, new CancellationTokenSource(2000)).Result;
                    failedToFindCapableDevice = false;
                    Logger.WriteInfo("Successfully connected to UPnP capable device with IP " + upnpDevice.GetExternalIPAsync().Result);
                }
                catch (Exception)
                {
                    if (!failedToFindCapableDevice)
                    {
                        Logger.WriteWarning("Failed to find UPnP capable device to port forward with.");
                    }
                    failedToFindCapableDevice = true;
                }
            }
        }

        public void ForwardPort(int port)
        {
            ports.Add(port);
            AttemptToForwardPortInternally(port);
        }

        public void RemovePort(int port)
        {
            ports.Remove(port);
            AttemptToRemovePortInternally(port);
        }

        protected void AttemptToForwardPortInternally(int port)
        {
            try
            {
                upnpDevice.CreatePortMapAsync(new Mapping(Protocol.Tcp, port, port, 30, "")).Wait();
                Logger.WriteInfo("Successfully forwarded port " + port);
            }
            catch
            {
                Logger.WriteWarning("Failed to forward port " + port);
            }
        }

        protected void AttemptToRemovePortInternally(int port)
        {
            try
            {
                upnpDevice.DeletePortMapAsync(upnpDevice.GetSpecificMappingAsync(Protocol.Tcp, port).Result).Wait();
                Logger.WriteInfo("Successfully removed forwarded port " + port);
            }
            catch
            {
                Logger.WriteWarning("Failed to removed forwarded port " + port);
            }
        }

        protected void CheckConnection(object sender, EventArgs args)
        {
            if(failedToFindCapableDevice)
            {
                FindUPnPDevice();
                if(!failedToFindCapableDevice)
                {
                    foreach(int port in ports)
                    {
                        AttemptToForwardPortInternally(port);
                    }
                }
            }
            else
            {
                try
                {
                    upnpDevice.GetExternalIPAsync().Wait();
                }
                catch
                {
                    Logger.WriteWarning("Lost connection to UPnP router.");
                    failedToFindCapableDevice = true;
                    FindUPnPDevice();
                    if (!failedToFindCapableDevice)
                    {
                        foreach (int port in ports)
                        {
                            AttemptToForwardPortInternally(port);
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            deviceReconnectTimer.Stop();
            
        }
    }
}
