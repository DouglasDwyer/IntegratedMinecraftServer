using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Player
{
    public class AllPlayerDisplay : PlayerDisplay
    {
        public AllPlayerDisplay()
        {
            Name = "All";
        }

        public override Type GetComponentType()
        {
            return typeof(AllPlayerDisplayView);
        }
    }
}
