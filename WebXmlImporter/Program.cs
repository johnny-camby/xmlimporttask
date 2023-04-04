using Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace WebXmlImporter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration(args);

            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .CreateLogger();


            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var ctx = scope.ServiceProvider.GetService<XmlImporterDbContext>();
                    ctx.Database.EnsureDeleted();
                    ctx.Database.Migrate();
                }
                catch (Exception ex)
                {
                    //var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    //logger.LogError(ex, "An error occurred while migrating the database");
                    Log.Fatal(ex, "An error occurred while migrating the database");
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
            host.Run();
        }

        private static IConfiguration GetConfiguration(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("serilog.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"serilog.{environment}.json", optional: true, reloadOnChange: true);

            configurationBuilder.AddCommandLine(args);
            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, configApp) =>
            {
                var configurationRoot = configApp.Build();

                configApp.AddJsonFile("serilog.json", optional: true, reloadOnChange: true);

                var env = hostContext.HostingEnvironment;

                configApp.AddJsonFile($"serilog.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                configApp.AddCommandLine(args);
                configApp.AddEnvironmentVariables();
            })
            .ConfigureWebHostDefaults(webBuilder => 
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog((hostContext, loggerConfig) =>
             {
                 loggerConfig
                     .ReadFrom.Configuration(hostContext.Configuration)
                     .Enrich.WithProperty("ApplicationName", hostContext.HostingEnvironment.ApplicationName);
             });
    }
}
