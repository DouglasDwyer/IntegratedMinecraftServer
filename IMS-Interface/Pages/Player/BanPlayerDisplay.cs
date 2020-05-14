using IMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Pages.Player
{
    public class BanPlayerDisplay : PlayerDisplay
    {
        public BanPlayerDisplay()
        {
            Name = "Banned";
        }

        public override bool ShouldDisplayFor(ServerProxy server) => server.SupportsBanning;

        public override Type GetComponentType()
        {
            return typeof(BanPlayerDisplayView);
        }
    }
}
