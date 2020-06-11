using System;
using System.Collections.Generic;
using System.Linq;
using IMS_Library;

namespace IMS_Interface.Preferences
{
    public class ConditionalDisplay : PreferenceDisplay
    {
        public Func<IMSConfiguration, bool> Condition;
        public PreferenceDisplay InternalDisplay;

        public ConditionalDisplay() { }
        public ConditionalDisplay(PreferenceDisplay internalDisplay, Func<IMSConfiguration, bool> condition) : base(null, null, null)
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
