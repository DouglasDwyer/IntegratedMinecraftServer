using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Shared.Preferences
{
    public class ConditionalDisplay : PreferenceDisplay
    {
        public Func<bool> Condition;
        public PreferenceDisplay InternalDisplay;

        public ConditionalDisplay() { }
        public ConditionalDisplay(PreferenceDisplay internalDisplay, Func<bool> condition) : base(null, null, null)
        {
            InternalDisplay = internalDisplay;
            Condition = condition;
        }

        public override Type GetComponentType()
        {
            return typeof(ConditionalDisplayView);
        }
    }
}
