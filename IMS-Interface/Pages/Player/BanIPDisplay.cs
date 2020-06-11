using IMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Player
{
    public class BanIPDisplay : PlayerDisplay
    {
        public BanIPDisplay()
        {
            Name = "Banned IPs";
        }

        public override bool ShouldDisplayFor(ServerProxy server) => server.SupportsBanningIPs;

        public override Type GetComponentType()
        {
            return typeof(BanIPDisplayView);
        }
    }
}
