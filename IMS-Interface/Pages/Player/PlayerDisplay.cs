using IMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Player
{
    public class PlayerDisplay
    {
        public string Name;

        public virtual Type GetComponentType()
        {
            return typeof(PlayerDisplayView);
        }

        public virtual bool ShouldDisplayFor(ServerProxy server)
        {
            return true;
        }
    }
}
