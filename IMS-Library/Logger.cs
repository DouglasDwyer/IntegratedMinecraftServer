using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    public static class Logger
    {
        private static StreamWriter logWriter;

        static Logger()
        {
            if(!Directory.Exists(Constants.ExecutionPath + Constants.LogLocation))
            {
                Directory.CreateDirectory(Constants.ExecutionPath + Constants.LogLocation);
            }
            logWriter = new StreamWriter(Constants.ExecutionPath + Constants.LogLocation + "/IMS." + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".txt", false);
            logWriter.AutoFlush = true;
        }

        public static void WriteInfo(string information)
        {
            lock (logWriter)
            {
                Console.WriteLine("[INFO] [" + DateTime.Now + "] " + information);
                logWriter.WriteLine("[INFO] [" + DateTime.Now + "] " + information);
            }
        }

        public static void WriteWarning(string warning)
        {
            lock (logWriter)
            {
                Console.WriteLine("[WARNING] [" + DateTime.Now + "] " + warning);
                logWriter.WriteLine("[WARNING] [" + DateTime.Now + "] " + warning);
            }
        }

        public static void WriteError(string error)
        {
            lock (logWriter)
            {
                Console.WriteLine("[ERROR] [" + DateTime.Now + "] " + error);
                logWriter.WriteLine("[ERROR] [" + DateTime.Now + "] " + error);
            }
        }

        public static void FinishLog()
        {
            lock (logWriter)
            {
                logWriter.WriteLine("[INFO] Exited cleanly.");
                logWriter.Close();
            }
        }
    }
}
