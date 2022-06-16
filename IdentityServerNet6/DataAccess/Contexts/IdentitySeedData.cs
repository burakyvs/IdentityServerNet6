using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServerNet6.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerNet6.DataAccess.Contexts
{
    public static class IdentitySeedData
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            using (var dbContext = serviceProvider.GetRequiredService<IdentityDbContext>())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();

                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                if (!userManager.Users.Any())
                {
                    userManager.CreateAsync(new ApplicationUser { FullName = "Burak Yavas", Email = "burakyvs0@gmail.com", UserName = "burakyvs"}, "Test123.").Wait();
                }
            }
        }

        public static void SeedResources(IServiceProvider serviceProvider)
        {
            using (var persistedDbContext = serviceProvider.GetRequiredService<PersistedGrantDbContext>())
            {
                persistedDbContext.Database.EnsureCreated();
                persistedDbContext.Database.Migrate();

                using (var configDbContext = serviceProvider.GetRequiredService<ConfigurationDbContext>())
                {
                    configDbContext.Database.EnsureCreated();
                    configDbContext.Database.Migrate();

                    if (!configDbContext.Clients.Any())
                    {
                        foreach (var client in Config.ClientManager.Clients)
                        {
                            configDbContext.Clients.Add(client.ToEntity());
                        }
                        configDbContext.SaveChanges();
                    }

                    if (!configDbContext.IdentityResources.Any())
                    {
                        foreach (var resource in Config.ResourceManager.IdentityResources)
                        {
                            configDbContext.IdentityResources.Add(resource.ToEntity());
                        }
                        configDbContext.SaveChanges();
                    }

                    if (!configDbContext.ApiResources.Any())
                    {
                        foreach (var resource in Config.ResourceManager.ApiResources)
                        {
                            configDbContext.ApiResources.Add(resource.ToEntity());
                        }
                        configDbContext.SaveChanges();
                    }

                    if (!configDbContext.ApiScopes.Any())
                    {
                        foreach (var resource in Config.ScopeManager.ApiScopes)
                        {
                            configDbContext.ApiScopes.Add(resource.ToEntity());
                        }
                        configDbContext.SaveChanges();
                    }
                }   
            }
        }
    }
}
