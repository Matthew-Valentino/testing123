using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bank4Us.ServiceApp
{
    public class Program
    {
        /// <summary>
        ///   Course Name: COSC 6360 Enterprise Architecture
        ///   Year: Fall 2021
        ///   Name: William J Leannah
        ///   Description: Example implementation of a Service App with MVC           
        /// </summary>
        /// 
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
