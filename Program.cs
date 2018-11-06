using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog.Core;
using Serilog;


namespace EPWebAPI
{
    public class Program
    {
        // public static IConfiguration Configuration {get;} = new ConfigurationBuilder()
        //     .SetBasePath(Directory.GetCurrentDirectory())
        //     .AddJsonFile("appsettings.json",optional:false, reloadOnChange:true)
        //     .Build();
        public static void Main(string[] args)
        {

            // Log.Logger = new LoggerConfiguration()
            //     .ReadFrom.Configuration(Configuration)
            //     .CreateLogger();
            try
            {
                //Log.Information("Starting Web Host");
                CreateWebHostBuilder(args).Build().Run();
                return;
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "Host Terminated unexpectedly");
            }
            finally
            {
                //Log.Information("Flushing Logs");
                //Log.CloseAndFlush();
            }
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
                // .UseSerilog();
           
    }
}
