using System;
using System.Diagnostics;
using IMS_Library;
using System.IO;

namespace IMS_Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            Process.Start(new ProcessStartInfo(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "http://localhost:8080"));
            IMS ims = new IMS();
            ims.WebServer = new IMSWebInterface();
            ims.SimulateService();
        }
    }
}
