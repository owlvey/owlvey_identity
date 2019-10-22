using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Owlvey.Falcon.Authority.Infra.Data.Sqlite.Contexts;
using Owvley.Falcon.Authority.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Authority.Infra.Data.Sqlite.Seed
{
    public static class DatabaseSeed
    {
        public static void InitializeDbTestData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<FalconAuthDbContext>().Database.Migrate();

                var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                if (!configurationDbContext.Clients.Any())
                {
                    foreach (var client in Clients.Get())
                    {
                        configurationDbContext.Clients.Add(client.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }

                if (!configurationDbContext.IdentityResources.Any())
                {
                    foreach (var resource in Resources.GetIdentityResources())
                    {
                        configurationDbContext.IdentityResources.Add(resource.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }

                if (!configurationDbContext.ApiResources.Any())
                {
                    foreach (var resource in Resources.GetApiResources())
                    {
                        configurationDbContext.ApiResources.Add(resource.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }

                var dbContext = scope.ServiceProvider.GetRequiredService<FalconAuthDbContext>();


                string[] roles = new string[] { "Admin", "Guest" };

                foreach (string role in roles)
                {
                    var roleStore = new RoleStore<IdentityRole>(dbContext);

                    if (!dbContext.Roles.Any(r => r.Name == role))
                    {
                        var success = roleStore.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                    }
                }
                
                if (!dbContext.Users.Any())
                {

                    foreach (var testUser in Users.Get())
                    {
                        if (!dbContext.Users.Any(c => c.UserName.Equals(testUser.UserName))) {
                            var identityUser = new User()
                            {
                                Id = Guid.NewGuid().ToString(),
                                UserName = testUser.UserName,
                                NormalizedUserName = testUser.UserName.ToUpper(),
                                NormalizedEmail = testUser.Email.ToUpper(),
                                FirstName = testUser.FirstName,
                                LastName = testUser.LastName,
                                Email = testUser.Email,
                                EmailConfirmed = true,
                                PhoneNumberConfirmed = true,
                                SecurityStamp = Guid.NewGuid().ToString("D")
                            };

                            var password = new PasswordHasher<User>();
                            var hashed = password.HashPassword(identityUser, "P@$$w0rd");
                            identityUser.PasswordHash = hashed;

                            var userStore = new UserStore<User>(dbContext);
                            var result = userStore.CreateAsync(identityUser).GetAwaiter().GetResult();
                            
                        }
                    }
                }
            }
        }
    }
}
