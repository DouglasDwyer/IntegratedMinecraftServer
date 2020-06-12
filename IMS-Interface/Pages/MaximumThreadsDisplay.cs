using System;
using System.Collections.Generic;
using System.Linq;
using IMS_Interface.Preferences;
using IMS_Library;

namespace IMS_Interface
{
    /// <summary>
    /// Represents a preference that determines the maximum number of worker threads that a Minecraft server can allocate.
    /// </summary>
    public class MaximumThreadDisplay : PreferenceDisplay
    {
        /// <summary>
        /// Creates a new maximum threads preference display with the specified data.
        /// </summary>
        /// <param name="name">The name of the field to read/write on the <see cref="IMSConfiguration"/> object.</param>
        /// <param name="display">The name that should display on the admin console.</param>
        /// <param name="description">The description that should appear in the hover-over tooltip.</param>
        public MaximumThreadDisplay(string name, string display, string description) : base(name, display, description)
        {
        }

        /// <summary>
        /// Retrieves the displayview type for this preference display.
        /// </summary>
        /// <returns>The <see cref="Type"/> of <see cref="PreferenceDisplayView"/> that this display corresponds to.</returns>
        public override Type GetComponentType()
        {
            return typeof(MaximumThreadsDisplayView);
        }
    }
}
