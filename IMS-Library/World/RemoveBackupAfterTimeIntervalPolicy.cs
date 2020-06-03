using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    public class RemoveBackupAfterTimeIntervalPolicy : IBackupPolicy
    {
        public string BackupName = "Automatic backup";
        public TimeSpan BackupRemovalTime = TimeSpan.FromHours(72);

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
