using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    /// <summary>
    /// Represents the IMS admin console web interface.
    /// </summary>
    public abstract class WebInterface
    {
        /// <summary>
        /// The port which the interface should run on.
        /// </summary>
        public WebPort Port;

        /// <summary>
        /// This method starts the web interface.
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// This method shuts down the web interface.
        /// </summary>
        public abstract void Stop();
    }
}
