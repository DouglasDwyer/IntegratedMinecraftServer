using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.World
{
    public class WorldDisplay
    {
        public string Name;

        public virtual Type GetComponentType()
        {
            return typeof(WorldDisplayView);
        }
    }
}
