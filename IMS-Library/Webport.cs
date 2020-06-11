using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    /// <summary>
    /// Represents a port on the host computer, and contains data about the port's identity and whether it should be forwarded.
    /// </summary>
    [Serializable]
    public struct WebPort
    {
        /// <summary>
        /// The port number that this object represents.
        /// </summary>
        public int Port;
        /// <summary>
        /// Whether IMS should attempt to forward this port using UPnP when it is in use.
        /// </summary>
        public bool AttemptUPnPForwarding;

        /// <summary>
        /// Creates a new <see cref="WebPort"/> instance with the specified port that does not attempt UPnP forwarding.
        /// </summary>
        /// <param name="port">The port this object represents.</param>
        public WebPort(int port)
        {
            Port = port;
            AttemptUPnPForwarding = false;
        }

        /// <summary>
        /// Creates a new <see cref="WebPort"/> instance.
        /// </summary>
        /// <param name="port">The port that this object represents.</param>
        /// <param name="attemptUPnP">Whether IMS should attempt to forward the port.</param>
        public WebPort(int port, bool attemptUPnP)
        {
            Port = port;
            AttemptUPnPForwarding = attemptUPnP;
        }
    }
}
