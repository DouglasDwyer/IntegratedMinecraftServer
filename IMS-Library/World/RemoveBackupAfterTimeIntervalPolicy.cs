using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    /// <summary>
    /// This represents a backup policy which removes backups of a specified name after they are a certain age.
    /// </summary>
    public class RemoveBackupAfterTimeIntervalPolicy : IBackupPolicy
    {
        /// <summary>
        /// The name of the backups to remove.
        /// </summary>
        public string BackupName = "Automatic backup";
        /// <summary>
        /// The age that any one backup needs to be in order to be automatically deleted.
        /// </summary>
        public TimeSpan BackupRemovalTime = TimeSpan.FromHours(72);

        /// <summary>
        /// This method causes the backup policy to iterate over known backups and delete the ones which are older than <see cref="BackupRemovalTime"/>.
        /// </summary>
        /// <param name="world">The world whose backups to operate on.</param>
        public void Update(World world)
        {
            foreach(BackupInformation backup in world.Backups.Values)
            {
                if(backup.Name == BackupName && backup.Date + BackupRemovalTime < DateTime.Now)
                {
                    world.DeleteBackupAsync(backup.ID);
                }
            }
        }
    }
}
