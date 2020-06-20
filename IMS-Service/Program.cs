using System;
using System.Diagnostics;
using IMS_Library;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using IMS_Interface;
using System.Runtime.InteropServices;
using System.Linq;
using System.ServiceProcess;
using WindowManager;

namespace IMS_Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            IMS ims = new IMS();
            ims.WebServer = new IMSWebInterface();
            Login.CredentialsResetter = new CredentialResetter();
            Directory.SetCurrentDirectory(Constants.ExecutionPath);
            if (args.Contains("-devmode"))
            {
                AllocConsole();
                ims.SimulateService();
            }
            else if (args.Contains("-run"))
            {
                ServiceBase.Run(ims);
            }
            else
            {
                if(new ServiceController("IMS").Status == ServiceControllerStatus.Running)
                {
                    StartProcess("cmd.exe", "/C start http://127.0.0.1:" + new IMSSettings().FromConfiguration().ManagementPort.Port);
                }
                else
                {
                    if(Interaction.MsgBox("IMS is not currently running.  Would you like to start IMS?", "Start IMS?", MsgBoxStyle.YesNo) == MsgBoxResult.Yes)
                    {
                        StartProcess("cmd.exe", "/C sc start IMS");
                        StartProcess("cmd.exe", "/C start http://127.0.0.1:" + new IMSSettings().FromConfiguration().ManagementPort.Port);
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
            }
        }

        private static void StartProcess(string file, string arguments)
        {
            Process.Start(new ProcessStartInfo { FileName = file, Arguments = arguments, UseShellExecute = false, CreateNoWindow = true });
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();
    }
}
