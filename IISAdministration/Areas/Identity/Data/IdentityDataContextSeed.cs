using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace IISAdministration.Areas.Identity.Data
{
    public class IdentityDataContextSeed
    {
        private const string SuperAdminEmail = "admin@test.com";
        private const string SuperAdminPassword = "DefaultPassword123!";
        public static async Task SeedAsync(IdentityDataContext context, UserManager<IdentityUser> userManager)
        {

            await context.Database.EnsureCreatedAsync();

            var existingSuperAdmin = await userManager.FindByEmailAsync(SuperAdminEmail);
            if (existingSuperAdmin == null)
            {
                var defaultUser = new IdentityUser(SuperAdminEmail) { Email = SuperAdminEmail };
                var result = await userManager.CreateAsync(defaultUser, SuperAdminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(",", result.Errors));
                }
            }
        }
    }
}
