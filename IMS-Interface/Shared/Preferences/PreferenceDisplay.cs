using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Preferences
{
    public class PreferenceDisplay
    {
        public PreferenceEditor ParentDisplay;
        public string FieldName;
        public string DisplayName;
        public string Description;

        public PreferenceDisplay() { }
        public PreferenceDisplay(string name, string display, string description)
        {
            FieldName = name;
            DisplayName = display;
            Description = description;
        }

        public virtual Type GetComponentType()
        {
            return typeof(PreferenceDisplayView);
        }
    }
}
