using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    public abstract class IMSPluginBase
    {
        public IMS Service { get => IMS.Instance; }

        public abstract string Name { get; }
        public abstract Version CurrentVersion { get; }
        public abstract string Description { get; }

        public virtual void Start() { }
        public virtual void Stop() { }
    }
}
