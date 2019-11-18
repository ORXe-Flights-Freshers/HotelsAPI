using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HotelAPI.Core.Service;
using HotelAPI.Core.Exceptions;

namespace HotelAPI
{
    public class Programs
    {
        public static void Main(string[] args)
        {
            //try
            //{
            //    FirebaseService.Authenticate();
            //}
            //catch (InvalidHostException)
            //{
            //    Console.WriteLine("Exception");
            //    return;
            //}
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(urls: "http://*:5000");
    }
}
