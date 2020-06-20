using System;
using System.Collections;
using System.Collections.Generic;

namespace IMS_Library
{
    /// <summary>
    /// Provides the ability to send simple information, warning, and error messages to the user in case anything goes wrong.
    /// These messages should be sent sparingly to avoid overwhelming users.
    /// </summary>
    public sealed class InformationController : IMSConfiguration
    {
        /// <summary>
        /// A list which contains all of the messages that the admin console should display to the user.
        /// </summary>
        public List<InformationItem> Messages = new List<InformationItem>();
        /// <summary>
        /// Whether there are new, unread messages for the user.
        /// </summary>
        public bool NewMessagesForUser = false;

        private object Locker = new object();

        /// <summary>
        /// Creates an info message to show to the user.  Info messages simply provide some data to the user; they do not indicate any errors in IMS.
        /// </summary>
        /// <param name="message">The message to show.</param>
        /// <param name="log">If true, the controller will also write the message to <see cref="Logger"/>.</param>
        public void LogInfo(string message, bool log = true)
        {
            if(log)
            {
                Logger.WriteInfo(message);
            }
            lock(Locker)
            {
                InformationItem item = Messages.Find(x => x.Message == message);
                if (item is null)
                {
                    Messages.Add(new InformationItem(message, DateTime.Now, InformationItem.MessageType.Info));
                    NewMessagesForUser = true;
                }
                else
                {
                    item.Severity = InformationItem.MessageType.Info;
                    item.LastSendTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Creates a warning message to show to the user.  Warning messages provide a warning to the user about circumstances that may cause IMS to function irregularly.
        /// </summary>
        /// <param name="message">The message to show.</param>
        /// <param name="log">If true, the controller will also write the message to <see cref="Logger"/>.</param>
        public void LogWarning(string message, bool log = true)
        {
            if (log)
            {
                Logger.WriteWarning(message);
            }
            lock (Locker)
            {
                InformationItem item = Messages.Find(x => x.Message == message);
                if (item is null)
                {
                    Messages.Add(new InformationItem(message, DateTime.Now, InformationItem.MessageType.Warning));
                    NewMessagesForUser = true;
                }
                else
                {
                    item.Severity = InformationItem.MessageType.Warning;
                    item.LastSendTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Creates an error message to show to the user.  Error messages provides an error to the user about a specific event that impacted the performance of IMS.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="log">If true, the controller will also write the message to <see cref="Logger"/>.</param>
        public void LogError(string message, bool log = true)
        {
            if (log)
            {
                Logger.WriteError(message);
            }
            lock (Locker)
            {
                InformationItem item = Messages.Find(x => x.Message == message);
                if (item is null)
                {
                    Messages.Add(new InformationItem(message, DateTime.Now, InformationItem.MessageType.Error));
                    NewMessagesForUser = true;
                }
                else
                {
                    item.Severity = InformationItem.MessageType.Error;
                    item.LastSendTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Retrieves the location of the file which stores user information messages.
        /// </summary>
        /// <returns>The absolute path of the data file.</returns>
        public override string GetDefaultFilePath()
        {
            return Constants.ExecutionPath + Constants.DataLocation + "/usermessages.xml";
        }
    }
}
