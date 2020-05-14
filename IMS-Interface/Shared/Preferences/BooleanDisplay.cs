using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Shared.Preferences
{
    public class BooleanDisplay : PreferenceDisplay
    {
        public string WhenTrue, WhenFalse;

        public BooleanDisplay(string name, string display, string description, string whenTrue, string whenFalse) : base(name, display, description)
        {
            WhenTrue = whenTrue;
            WhenFalse = whenFalse;
        }

        public override Type GetComponentType()
        {
            return typeof(BooleanDisplayView);
        }
    }
}
