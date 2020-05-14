using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    [Serializable]
    public struct WebPort
    {
        public int Port;
        public bool AttemptUPnPForwarding;

        public WebPort(int port)
        {
            Port = port;
            AttemptUPnPForwarding = false;
        }

        public WebPort(int port, bool attemptUPnP)
        {
            Port = port;
            AttemptUPnPForwarding = attemptUPnP;
        }
    }
}
