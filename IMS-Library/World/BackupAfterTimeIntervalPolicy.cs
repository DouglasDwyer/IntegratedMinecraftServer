using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    public class BackupAfterTimeIntervalPolicy : IBackupPolicy
    {
        public string BackupName = "Automatic backup";

        public TimeSpan BackupInterval = TimeSpan.FromHours(24);
        public DateTime LastBackedUp = DateTime.Now;

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
