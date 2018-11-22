using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;



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
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                //Log.Information("Flushing Logs");
                //Log.CloseAndFlush();
            }
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseIISIntegration()
                .UseStartup<Startup>();
                // .UseSerilog();
           
    }
}
