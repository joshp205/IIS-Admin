using IISAdministration.Areas.Identity.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IISAdministration
{

    public class Program
    {

        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).UseIISIntegration().Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var identityContext = services.GetRequiredService<IdentityDataContext>();
                    var UserManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    await IdentityDataContextSeed.SeedAsync(identityContext, UserManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
