using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    /// <summary>
    /// Represents a backup policy where a backup is created when a specific time interval elapses.
    /// </summary>
    public class BackupAfterTimeIntervalPolicy : IBackupPolicy
    {
        /// <summary>
        /// The name of the backup to create.
        /// </summary>
        public string BackupName = "Automatic backup";

        /// <summary>
        /// The interval between world backups.
        /// </summary>
        public TimeSpan BackupInterval = TimeSpan.FromHours(24);
        /// <summary>
        /// The last time the world was backed up.  When it is <see cref="BackupInterval"/> past this time, a new backup is made.
        /// </summary>
        public DateTime LastBackedUp = DateTime.Now;

        /// <summary>
        /// This function checks to determine whether it is time to make a backup, and does so if necessary.
        /// </summary>
        /// <param name="world">The parent world.</param>
        public void Update(World world)
        {
            if(LastBackedUp + BackupInterval < DateTime.Now)
            {
                world.MakeBackupAsync(BackupName);
                LastBackedUp = DateTime.Now;
            }
        }
    }
}
