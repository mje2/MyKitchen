using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyKitchen.Data;
using MyKitchen.Models;

namespace MyKitchen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).ConfigureLogging(x =>
            {
                x.ClearProviders();
                x.AddApplicationInsights();
            }).Build();

            using (var scope = host.Services.CreateScope())
            {

                var services = scope.ServiceProvider;

                try
                {
                    //TODO RESEARCH - learn how to use scopes and possibly DI into a class outside of a controller.
                    //for now we will pass in the required dependencies for the DBInitializer in the constructor.

                    var context = services.GetRequiredService<MyKitchen.Data.ApplicationDbContext>();
                    var userManager = services.GetService<UserManager<ApplicationUser>>();
                    var foodItemRepository = services.GetService<IFoodItemRepository>();

                    DbInitializer.Initialize(context,userManager,foodItemRepository);

                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured while seeding the database.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration((context,config) =>
                    {
                        var builtConfig = config.Build();

                        var azureServicetokenProvider = new AzureServiceTokenProvider();
                        var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServicetokenProvider.KeyVaultTokenCallback));

                        var keyuri = $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/";

                        config.AddAzureKeyVault(keyuri,keyVaultClient,new DefaultKeyVaultSecretManager());

                    }).UseStartup<Startup>();
    }
}
