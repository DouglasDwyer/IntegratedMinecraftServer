using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    public class BackupAfterTimeIntervalPolicy : IBackupPolicy
    {
        public TimeSpan BackupInterval = TimeSpan.FromHours(1);
        public DateTime LastBackedUp;

        public void Update(World world)
        {
            if(LastBackedUp + BackupInterval < DateTime.Now)
            {
                world.MakeBackupAsync();
                LastBackedUp = DateTime.Now;
            }
        }
    }
}
