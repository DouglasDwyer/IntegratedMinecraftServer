using IMS_Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Service
{
    public class IMSWebInterface : WebInterface
    {
        public override void Start()
        {
            if(Port.AttemptUPnPForwarding)
            {
                IMS.Instance.PortManager.ForwardPort(Port.Port);
            }
            IMS.Instance.FirewallManager.CreateFirewallPortException(Port.Port);
            IMS_Interface.Program.Start(Port.Port);
        }

        public override void Stop()
        {
            IMS_Interface.Program.Stop();
            IMS.Instance.FirewallManager.RemoveFirewallPortException(Port.Port);
            if (Port.AttemptUPnPForwarding)
            {
                IMS.Instance.PortManager.RemovePort(Port.Port);
            }
        }
    }
}
