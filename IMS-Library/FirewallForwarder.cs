using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    public class FirewallForwarder
    {
        public void CreateFirewallExecutableException(string name, string path)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C netsh advfirewall firewall add rule name=\"IMS E" + name + "\" dir=in protocol=tcp program=" + path + " profile=any action=allow";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }

        public void RemoveFirewallExecutableException(string name)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C netsh advfirewall firewall delete rule name=\"IMS E" + name + "\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }

        public void CreateFirewallPortException(int port)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C netsh advfirewall firewall add rule name=\"IMS P" + port + "\" dir=in protocol=tcp localport=" + port + " profile=any action=allow";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }

        public void RemoveFirewallPortException(int port)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C netsh advfirewall firewall delete rule name=\"IMS P" + port + "\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }
    }
}
