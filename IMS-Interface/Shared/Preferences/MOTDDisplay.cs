using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Preferences
{
    public class MOTDDisplay : PreferenceDisplay
    {
        public MOTDDisplay(string name, string display, string description) : base(name, display, description)
        {
        }

        public override Type GetComponentType()
        {
            return typeof(MOTDDisplayView);
        }
    }
}
