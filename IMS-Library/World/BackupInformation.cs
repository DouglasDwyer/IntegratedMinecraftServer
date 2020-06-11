using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    /// <summary>
    /// This class is used to keep track of backup data.
    /// </summary>
    public class BackupInformation
    {
        /// <summary>
        /// The display name of the backup.
        /// </summary>
        public string Name;
        /// <summary>
        /// The time at which the backup was created.
        /// </summary>
        public DateTime Date;
        /// <summary>
        /// The unique identifier of the backup.
        /// </summary>
        public Guid ID;
    }
}
