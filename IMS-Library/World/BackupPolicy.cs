using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    public class BackupPolicy
    {
        public ServerProxy Server { get; set; }

        public virtual void Initialize() { }
        public virtual void Update() { }
    }
}
