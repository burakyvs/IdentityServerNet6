using IdentityServerNet6.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerNet6.DataAccess.Contexts
{
    public static class IdentitySeedData
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var dbContext = serviceProvider.GetRequiredService<IdentityDbContext>())
            {
                dbContext.Database.Migrate();

                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                if (!userManager.Users.Any())
                {
                    userManager.CreateAsync(new ApplicationUser { FullName = "Burak Yavas", Email = "burakyvs0@gmail.com", UserName = "burakyvs"}, "Test123.").Wait();
                }
            }
        }
    }
}
