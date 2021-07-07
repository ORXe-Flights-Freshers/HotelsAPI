using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using HotelAPI.Core.Service;
using HotelAPI.HotelAPI.Core.Exceptions;

namespace HotelAPI
{
    public class Programs
    {
        public static void Main(string[] args)
        {
            try
            {
                FirebaseService.Authenticate();
            }
            catch (InvalidHostException)
            {
                Console.WriteLine("Exception");
                return;
            }
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(urls: "http://*:5000");
    }
}
