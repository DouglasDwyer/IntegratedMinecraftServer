using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Preferences
{
    public class DoubleDisplay : PreferenceDisplay
    {
        public double Minimum;
        public double Maximum;

        public DoubleDisplay() { }
        public DoubleDisplay(string name, string display, string description, double min, double max) : base(name, display, description)
        {
            Minimum = min;
            Maximum = max;
        }

        public override Type GetComponentType()
        {
            return typeof(DoubleDisplayView);
        }
    }
}
