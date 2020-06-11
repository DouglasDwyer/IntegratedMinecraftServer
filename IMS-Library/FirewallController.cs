using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    /// <summary>
    /// This is a manager class which provides methods to add/remove Windows firewall exceptions.
    /// </summary>
    public sealed class FirewallController
    {
        /// <summary>
        /// Allows an executable through the Windows firewall.
        /// </summary>
        /// <param name="name">The name of the exception to create.  This name should be unique (and probably should contain a <see cref="Guid"/>).</param>
        /// <param name="path">The path of the executable to allow through the Windows firewall.</param>
        public void CreateFirewallExecutableException(string name, string path)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C netsh advfirewall firewall add rule name=\"IMS E" + name + "\" dir=in protocol=tcp program=" + path.Replace("/","\\") + " profile=any action=allow";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }

        /// <summary>
        /// Removes a Windows firewall exception for the specified executable.
        /// </summary>
        /// <param name="name">The name of the exception to remove.</param>
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

        /// <summary>
        /// Allows a port through the Windows firewall.
        /// </summary>
        /// <param name="port">The port number to allow through the firewall.</param>
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

        /// <summary>
        /// Removes a port exception from the Windows firewall.
        /// </summary>
        /// <param name="port">The port number to remove.</param>
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
