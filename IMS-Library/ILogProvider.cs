using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library
{
    /// <summary>
    /// Represents a service which creates and maintains logfiles.  This interface may be used to read logfile information.
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// Retrieves all of the logfiles that are currently stored.
        /// </summary>
        /// <returns>A list of logfiles.</returns>
        public IEnumerable<LogFileInformation> GetAllLogFiles();
        /// <summary>
        /// Retrieves the content of a specific logfile.
        /// </summary>
        /// <param name="information">The logfile to read.</param>
        /// <returns>The text contained within the logfile.</returns>
        public string GetLogFile(LogFileInformation information);
        /// <summary>
        /// Deletes the logfile, removing it from disk.
        /// </summary>
        /// <param name="information">The logfile to delete.</param>
        public void DeleteLogFile(LogFileInformation information);
    }
}
