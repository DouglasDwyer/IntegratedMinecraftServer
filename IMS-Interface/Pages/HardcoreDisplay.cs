using IMS_Interface.Shared.Preferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Pages
{
    public class HardcoreDisplay : PreferenceDisplay
    {
        public string HardcoreFieldName;
        public string[] Values;

        public HardcoreDisplay(string name, string hardcoreName, string display, string description, string[] values) : base(name, display, description)
        {
            HardcoreFieldName = hardcoreName;
            Values = values;
        }

        public override Type GetComponentType()
        {
            return typeof(HardcoreDisplayView);
        }
    }
}
