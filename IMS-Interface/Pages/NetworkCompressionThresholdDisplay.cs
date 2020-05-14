using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS_Interface.Shared.Preferences;

namespace IMS_Interface.Pages
{
    public class NetworkCompressionThresholdDisplay : PreferenceDisplay
    {
        public NetworkCompressionThresholdDisplay(string name, string display, string description) : base(name, display, description)
        {
        }

        public override Type GetComponentType()
        {
            return typeof(NetworkCompressionThresholdDisplayView);
        }
    }
}
