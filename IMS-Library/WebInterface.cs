using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    public abstract class WebInterface
    {
        public WebPort Port;

        public abstract void Start();
        public abstract void Stop();
    }
}
