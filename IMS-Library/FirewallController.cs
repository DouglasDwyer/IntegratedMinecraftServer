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
        private Dictionary<string, string> SavedExecutablePaths = new Dictionary<string,string>();

        /// <summary>
        /// Allows an executable through the Windows firewall.
        /// </summary>
        /// <param name="name">The name of the exception to create.  This name should be unique (and probably should contain a <see cref="Guid"/>).</param>
        /// <param name="path">The path of the executable to allow through the Windows firewall.</param>
        public void CreateFirewallExecutableException(string name, string path)
        {
            lock (SavedExecutablePaths)
            {
                string error;
                if (!SavedExecutablePaths.ContainsValue(path))
                {
                    Extensions.ExecuteShellCommand("netsh advfirewall firewall delete rule name=all program=\"" + path.Replace("/", "\\") + "\"");
                }
                if(!SavedExecutablePaths.ContainsKey(name))
                {
                    SavedExecutablePaths.Add(name, path);
                }
                if (Extensions.ExecuteShellCommand("netsh advfirewall firewall add rule name=\"IMS E" + name + "\" dir=in protocol=tcp program=\"" + path.Replace("/", "\\") + "\" profile=any action=allow", out error) != 0)
                {
                    IMS.Instance.UserMessageManager.LogWarning("IMS was unable to make an exception in the windows firewall.", false);
                    Logger.WriteWarning("IMS was unable to make an exception in the windows firewall for " + path + ".  Error:\n" + error);
                }
            }
        }

        /// <summary>
        /// Removes a Windows firewall exception for the specified executable.
        /// </summary>
        /// <param name="name">The name of the exception to remove.</param>
        public void RemoveFirewallExecutableException(string name)
        {
            lock (SavedExecutablePaths)
            {
                string error;
                SavedExecutablePaths.Remove(name);
                if (Extensions.ExecuteShellCommand("netsh advfirewall firewall delete rule name=\"IMS E" + name + "\"", out error) != 0)
                {
                    IMS.Instance.UserMessageManager.LogWarning("IMS was unable to remove an exception in the windows firewall.", false);
                    Logger.WriteWarning("IMS was unable to remove an exception in the windows firewall for " + name + ".  Error:\n" + error);
                }
            }
        }

        /// <summary>
        /// Allows a port through the Windows firewall.
        /// </summary>
        /// <param name="port">The port number to allow through the firewall.</param>
        public void CreateFirewallPortException(int port)
        {
            string error;
            if(Extensions.ExecuteShellCommand("netsh advfirewall firewall add rule name=\"IMS P" + port + "\" dir=in protocol=tcp localport=" + port + " profile=any action=allow", out error) != 0)
            {
                IMS.Instance.UserMessageManager.LogWarning("IMS was unable to make an exception in the windows firewall.", false);
                Logger.WriteWarning("IMS was unable to make an exception in the windows firewall for port " + port + ".  Error:\n" + error);
            }
        }

        /// <summary>
        /// Removes a port exception from the Windows firewall.
        /// </summary>
        /// <param name="port">The port number to remove.</param>
        public void RemoveFirewallPortException(int port)
        {
            string error;
            if(Extensions.ExecuteShellCommand("netsh advfirewall firewall delete rule name=\"IMS P" + port + "\"", out error) != 0)
            {
                IMS.Instance.UserMessageManager.LogWarning("IMS was unable to remove an exception in the windows firewall.", false);
                Logger.WriteWarning("IMS was unable to remove an exception in the windows firewall for port " + port + ".  Error:\n" + error);
            }
        }
    }
}
