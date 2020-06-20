using IMS_Interface.Preferences;
using IMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface
{
    /// <summary>
    /// Represents a display that allows users to select how old log files should be before they are automatically deleted.
    /// </summary>
    public class LogDeletionIntervalDisplay : PreferenceDisplay
    {
        /// <summary>
        /// Creates a new log deletion interval preference display with the specified data.
        /// </summary>
        /// <param name="name">The name of the field to read/write on the <see cref="IMSConfiguration"/> object.</param>
        /// <param name="display">The name that should display on the admin console.</param>
        /// <param name="description">The description that should appear in the hover-over tooltip.</param>
        public LogDeletionIntervalDisplay(string name, string display, string description) : base(name, display, description)
        {
        }

        /// <summary>
        /// Retrieves the displayview type for this preference display.
        /// </summary>
        /// <returns>The <see cref="Type"/> of <see cref="PreferenceDisplayView"/> that this display corresponds to.</returns>
        public override Type GetComponentType()
        {
            return typeof(LogDeletionIntervalDisplayView);
        }
    }
}
