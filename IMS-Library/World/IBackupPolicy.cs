using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    /// <summary>
    /// This interface is used to implement backup functionality - it represents a controller which performs an action related to maintaining world backups when <see cref="Update(World)"/> is called.
    /// </summary>
    public interface IBackupPolicy
    {
        /// <summary>
        /// This method is called by <see cref="WorldController"/> every 60 seconds to allow the backup policy to perform any actions it needs to complete.
        /// </summary>
        /// <param name="world">The world to perform backup operations on.</param>
        public void Update(World world);
    }
}
