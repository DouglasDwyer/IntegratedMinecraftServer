using IMS_Interface.Preferences;
using IMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Preferences
{
    /// <summary>
    /// Represents a preference display that can be used to edit a <see cref="Dictionary{String, String}"/>.
    /// </summary>
    public class StringDictionaryDisplay : PreferenceDisplay
    {
        /// <summary>
        /// Creates a new dictionary display preference with the specified settings.
        /// </summary>
        /// <param name="name">The name of the field to edit on the <see cref="IMSConfiguration"/> object.</param>
        /// <param name="display">The name of the preference to display to the user.</param>
        /// <param name="description">The description of the preference to show to the user in a tooltip.</param>
        public StringDictionaryDisplay(string name, string display, string description) : base(name, display, description) { }

        /// <summary>
        /// Gets the type of component associated with <see cref="StringDictionaryDisplay"/> to render.
        /// </summary>
        /// <returns>The type of the Blazor component to render.</returns>
        public override Type GetComponentType()
        {
            return typeof(StringDictionaryDisplayView);
        }
    }
}
