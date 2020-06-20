using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;

namespace IMS_Interface
{
    public static class Extensions
    {
        private static string PublicIP = null, LocalIP = null;

        /// <summary>
        /// Creates a <see cref="RenderFragment"/> that can be used to dynamically render the specified type.
        /// </summary>
        /// <param name="type">The type to render.</param>
        /// <param name="attributes">Attributes that the component should receive.</param>
        /// <returns>A <see cref="RenderFragment"/> that may be used to render the component to HTML.</returns>
        public static RenderFragment GetRenderFragment(this Type type, Dictionary<string, object> attributes = null)
        {
            if(attributes is null)
            {
                attributes = new Dictionary<string, object>();
            }
            return builder =>
            {
                builder.OpenComponent(0, type);
                builder.AddMultipleAttributes(0, attributes);
                builder.CloseComponent();
            };
        }

        public static bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }

        private static FieldInfo GetPrivateField(this Type t, String name)
        {
            const BindingFlags bf = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            FieldInfo fi;
            while ((fi = t.GetField(name, bf)) == null && (t = t.BaseType) != null) { }
            return fi;
        }

        /// <summary>
        /// Gets the public IPv4 address that IMS is currently running on, or null if no IP could be obtained.
        /// </summary>
        /// <returns>The public IP address.</returns>
        public static string GetPublicIPAddress()
        {
            if(PublicIP != null)
            {
                return PublicIP;
            }
            try
            {
                return PublicIP = new WebClient().DownloadString("http://ipv4.icanhazip.com").Trim();
            }
            catch(Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the local IPv4 address that IMS is currently running on, or null if no IP could be obtained.
        /// </summary>
        /// <returns>The local IP address.</returns>
        public static string GetLocalIPAddress()
        {
            if(LocalIP != null)
            {
                return LocalIP;
            }
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return LocalIP = ip.ToString();
                }
            }
            return null;
        }
    }
}
