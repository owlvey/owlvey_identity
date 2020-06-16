using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Owlvey.Falcon.Authority.Presentation
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Warning(string.Format("Identity Host Environment: {0}", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")));
                Log.Warning("Starting web host at " + DateTime.Now.ToLongTimeString());                
                BuildWebHost(args, configuration).Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }
        
        public static IWebHost BuildWebHost(string[] args,
            IConfiguration configuration) =>
            WebHost.CreateDefaultBuilder(args)            
            .UseConfiguration(configuration)
            .UseSerilog()
            .UseStartup<Startup>()            
            .Build();
    }
}
