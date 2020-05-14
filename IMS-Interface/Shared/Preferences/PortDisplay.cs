using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Shared.Preferences
{
    public class PortDisplay : PreferenceDisplay
    {
        public PortDisplay(string name, string display, string description) : base(name, display, description) { }

    public override Type GetComponentType()
        {
            return typeof(PortDisplayView);
        }
    }
}
