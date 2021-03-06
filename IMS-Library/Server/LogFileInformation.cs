﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    /// <summary>
    /// Contains information about a logfile.
    /// </summary>
    public struct LogFileInformation
    {
        /// <summary>
        /// The name of the logfile.
        /// </summary>
        public string Name;
        /// <summary>
        /// The time at which the logfile was created.
        /// </summary>
        public DateTime CreationDate;
        /// <summary>
        /// Whether the logfile is the result of a clean exit, or a crash.
        /// </summary>
        public bool CleanExit;
    }
}
