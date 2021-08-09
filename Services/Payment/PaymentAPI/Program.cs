using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace PaymentAPI
{
    public class Program
    {
        private static IConfiguration Configuration
        {
            get
            {
                string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";


                return new ConfigurationBuilder()
                        .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddJsonFile($"appsettings.{env}.json", optional: true)
                        .AddEnvironmentVariables()
                        .Build();
            }
        }
        public static void Main(string[] args)
        {
            var host = BuildWebHost(Configuration, args);

            Log.Logger = new LoggerConfiguration()
                     .WriteTo.Console()
                     .WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
                     .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                     .WriteTo.Seq("http://localhost:5341/")
                     .MinimumLevel.Information()
                     .CreateLogger();

            host.Run();
        }


        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateOnBuild = false;
                })
                .CaptureStartupErrors(false)
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
        }
    }
}
