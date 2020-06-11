using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.World
{
    public class WorldBackupsDisplay : WorldDisplay
    {
        public WorldBackupsDisplay()
        {
            Name = "Backups";
        }

        public override Type GetComponentType()
        {
            return typeof(WorldBackupsDisplayView);
        }
    }
}
