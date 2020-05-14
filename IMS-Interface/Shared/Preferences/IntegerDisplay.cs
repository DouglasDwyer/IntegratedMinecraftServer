using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Shared.Preferences
{
    public class IntegerDisplay : PreferenceDisplay
    {
        public int Minimum;
        public int Maximum;

        public IntegerDisplay() { }
        public IntegerDisplay(string name, string display, string description, int min, int max) : base(name, display, description)
        {
            Minimum = min;
            Maximum = max;
        }

        public override Type GetComponentType()
        {
            return typeof(IntegerDisplayView);
        }
    }
}
