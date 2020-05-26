using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Pages.World
{
    public class WorldBackupPolicyDisplay : WorldDisplay
    {
        public WorldBackupPolicyDisplay()
        {
            Name = "Policy";
        }

        public override Type GetComponentType()
        {
            return typeof(WorldBackupPolicyDisplayView);
        }
    }
}
