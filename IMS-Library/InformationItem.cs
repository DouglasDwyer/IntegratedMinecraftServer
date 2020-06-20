using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    /// <summary>
    /// Represents a piece of information that the end user should be aware of.  The <see cref="InformationController"/> class uses these to store info, warnings, and errors produced by IMS.
    /// </summary>
    public class InformationItem
    {
        /// <summary>
        /// The message to display to the end user.
        /// </summary>
        public string Message;
        /// <summary>
        /// The last time this message was logged by IMS.
        /// </summary>
        public DateTime LastSendTime;
        /// <summary>
        /// How critical this message is.
        /// </summary>
        public MessageType Severity;

        /// <summary>
        /// Represents the severity of any one <see cref="InformationItem"/>.
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// This message simply provides some data to the user; it does not indicate any errors in IMS.
            /// </summary>
            Info,
            /// <summary>
            /// This message provides a warning to the user about circumstances that may cause IMS to function irregularly.
            /// </summary>
            Warning,
            /// <summary>
            /// This message provides an error to the user about a specific event that impacted the performance of IMS.
            /// </summary>
            Error
        }
        
        /// <summary>
        /// Creates a new instance of <see cref="InformationItem"/> with default parameters.
        /// </summary>
        public InformationItem() { }

        /// <summary>
        /// Creates a new instance of <see cref="InformationItem"/> with the specified data.
        /// </summary>
        /// <param name="message">The message to display to the end user.</param>
        /// <param name="time">The last time this message was logged by IMS.</param>
        /// <param name="severity">How critical this message is.</param>
        public InformationItem(string message, DateTime time, MessageType severity)
        {
            Message = message;
            LastSendTime = time;
            Severity = severity;
        }
    }
}
