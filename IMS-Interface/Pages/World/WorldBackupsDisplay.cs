using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Pages.World
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
