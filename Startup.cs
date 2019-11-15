using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elastic.Apm.AspNetCore;
using HotelAPI.Configuration;
using HotelAPI.HotelAPI.Core.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;


namespace HotelAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            var elasticUri = Configuration["ElasticConfiguration:Uri"];
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                })
                .CreateLogger();
            
        }

        public IConfigurationRoot Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
            });
            services.AddSingleton<AppSettings>();
            services.Configure<AppSettings>(
          Configuration.GetSection(nameof(AppSettings)));
            services.AddSingleton<AppSettings>(sp =>
                sp.GetRequiredService<IOptions<AppSettings>>().Value);
            services.AddScoped<HotelService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
                              ILoggerFactory loggerFactory)
        {

            loggerFactory.AddSerilog();
            app.UseHsts();
            app.UseMiddleware<SerilogMiddleware>();
            app.UseCors("AllowAll");
            app.UseMvc();
        }
    }
}
