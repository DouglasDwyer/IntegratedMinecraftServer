using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    /// <summary>
    /// This class is used for keeping IMS logs and debugging.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Represents the full path of the current log file.
        /// </summary>
        public static string CurrentLogFile { get; private set; }
        private static StreamWriter logWriter;

        /// <summary>
        /// Returns a reasonably-large block of recent console output from the logger.
        /// </summary>
        public static string ConsoleText {
            get
            {
                lock (logWriter)
                {
                    string toReturn = string.Concat(ConsoleTextList.ToArray());
                    if (toReturn.EndsWith("\n"))
                    {
                        toReturn = toReturn.Remove(toReturn.Length - 1);
                    }
                    return toReturn;
                }
            }
        }
        private static List<string> ConsoleTextList = new List<string>();

        static Logger()
        {
            if(!Directory.Exists(Constants.ExecutionPath + Constants.LogLocation))
            {
                Directory.CreateDirectory(Constants.ExecutionPath + Constants.LogLocation);
            }
            logWriter = new StreamWriter(CurrentLogFile = Constants.ExecutionPath + Constants.LogLocation + "/IMS." + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".txt", false);
            logWriter.AutoFlush = true;
        }

        /// <summary>
        /// Writes the <paramref name="information"/> string to the IMS log under the "info" category.
        /// </summary>
        /// <param name="information">The information to write to the log.</param>
        public static void WriteInfo(string information)
        {
            lock (logWriter)
            {
                WriteOutput("[INFO] [" + DateTime.Now + "] " + information);
            }
        }

        /// <summary>
        /// Writes the <paramref name="warning"/> string to the IMS log under the "warning" category.
        /// </summary>
        /// <param name="warning">The warning to write to the log.</param>
        public static void WriteWarning(string warning)
        {
            lock (logWriter)
            {
                WriteOutput("[WARNING] [" + DateTime.Now + "] " + warning);
            }
        }

        /// <summary>
        /// Writes the <paramref name="error"/> string to the IMS log under the "error" category.
        /// </summary>
        /// <param name="error">The error to write to the log.</param>
        public static void WriteError(string error)
        {
            lock (logWriter)
            {
                WriteOutput("[ERROR] [" + DateTime.Now + "] " + error);
            }
        }

        /// <summary>
        /// Stops all logging and marks the log as successful (differentiating it from a crashed/incomplete log).
        /// </summary>
        public static void FinishLog()
        {
            lock (logWriter)
            {
                logWriter.WriteLine("[INFO] Exited cleanly.");
                logWriter.Close();
            }
        }

        private static void WriteOutput(string output)
        {
            Console.WriteLine(output);
            logWriter.WriteLine(output);
            ConsoleTextList.Add(output + "\n");
            while (ConsoleTextList.Count > 100)
            {
                ConsoleTextList.RemoveAt(0);
            }
        }
    }
}
