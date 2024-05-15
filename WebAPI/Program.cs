using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((context, config) => {
                        var settings = config.Build();
                        var keyVaultURL = settings["KeyVaultConfiguration:KeyVaultURL"];
                        var keyVaultClientId = settings["KeyVaultConfiguration:ClientId"];
                        var keyVaultClientSecret = settings["KeyVaultConfiguration:ClientSecret"];
                        config.AddAzureKeyVault(keyVaultURL, keyVaultClientId, keyVaultClientSecret, new DefaultKeyVaultSecretManager());
                    }).ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var appInsightsKey = hostingContext.Configuration["ApplicationInsights:InstrumentationKey"];
                        config.AddApplicationInsightsSettings(instrumentationKey: appInsightsKey);
                    }); ;
                    webBuilder.UseStartup<Startup>();
                });
    }
}
