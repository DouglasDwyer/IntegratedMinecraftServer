using IMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Preferences
{
    /// <summary>
    /// A display used to allow the user to upload their own, custom server binaries.
    /// </summary>
    public class UploadServerBinaryDisplay : PreferenceDisplay
    {
        /// <summary>
        /// The path that the file should be uploaded to.
        /// </summary>
        public Func<ServerConfiguration,string> FinalFilePath;
        /// <summary>
        /// The path where the file should be temporarily stored.
        /// </summary>
        public Func<ServerConfiguration, string> TemporaryFilePath;

        /// <summary>
        /// Creates a new upload binary display with the specified settings.
        /// </summary>
        /// <param name="display">The title of this display to show to the user.</param>
        /// <param name="description">A description of what this preference display does.</param>
        /// <param name="tempPath">A function that returns the path where the user's file should be temporarily stored while preferences are being edited.</param>
        /// <param name="finalPath">A function that returns the path that the user's file should be uploaded to.</param>
        public UploadServerBinaryDisplay(string display, string description, Func<ServerConfiguration, string> tempPath, Func<ServerConfiguration, string> finalPath) : base(null, display, description)
        {
            FinalFilePath = finalPath;
            TemporaryFilePath = tempPath;
        }

        /// <summary>
        /// Gets the type of component associated with <see cref="UploadServerBinaryDisplay"/> to render.
        /// </summary>
        /// <returns>The type of the Blazor component to render.</returns>
        public override Type GetComponentType()
        {
            return typeof(UploadServerBinaryDisplayView);
        }
    }
}
