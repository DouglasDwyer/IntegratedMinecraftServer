using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IMS_Interface
{
    public class Program
    {
        public static IHost ServerHost;

        public static void Main(string[] args)
        {
            throw new NotSupportedException("IMS is meant to be run as a service.");
        }

        public static void Start(int port)
        {
            ServerHost = CreateHostBuilder(new[] { "--urls=http://+:" + port }).Build();
            ServerHost.RunAsync();
        }

        public static void Stop()
        {
            ServerHost.StopAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
