using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    public interface IBackupPolicy
    {
        public void Update(World world);
    }
}
