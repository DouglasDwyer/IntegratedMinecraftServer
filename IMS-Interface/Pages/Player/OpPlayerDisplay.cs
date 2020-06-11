using IMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Player
{
    public class OpPlayerDisplay : PlayerDisplay
    {
        public OpPlayerDisplay()
        {
            Name = "Operators";
        }

        public override bool ShouldDisplayFor(ServerProxy server) => server.SupportsOps;

        public override Type GetComponentType()
        {
            return typeof(OpPlayerDisplayView);
        }
    }
}
