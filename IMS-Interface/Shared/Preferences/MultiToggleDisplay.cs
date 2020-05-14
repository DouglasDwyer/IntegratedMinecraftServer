using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Shared.Preferences
{
    public class MultiToggleDisplay : PreferenceDisplay
    {
        public string[] Values;
        public int IndexOffset = 0;

        public MultiToggleDisplay(string name, string display, string description, string[] values) : base(name, display, description)
        {
            Values = values;
        }

        public override Type GetComponentType()
        {
            return typeof(MultiToggleDisplayView);
        }
    }
}
