using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.World
{
    public class BackupPolicyDisplay
    {
        public Type ComponentType;
        public string Name;

        public BackupPolicyDisplay() { }

        public BackupPolicyDisplay(string name, Type type)
        {
            Name = name;
            ComponentType = type;
        }
    }
}
