using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Pages.Player
{
    public class OnlinePlayerDisplay : PlayerDisplay
    {
        public OnlinePlayerDisplay()
        {
            Name = "Online";
        }

        public override Type GetComponentType()
        {
            return typeof(OnlinePlayerDisplayView);
        }
    }
}
