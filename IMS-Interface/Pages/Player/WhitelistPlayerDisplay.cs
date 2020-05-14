using IMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Pages.Player
{
    public class WhitelistPlayerDisplay : PlayerDisplay
    {
        public WhitelistPlayerDisplay()
        {
            Name = "Whitelist";
        }

        public override Type GetComponentType()
        {
            return typeof(WhitelistPlayerDisplayView);
        }

        public override bool ShouldDisplayFor(ServerProxy server)
        {
            return server.SupportsWhitelist;
        }
    }
}
