using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Owlvey.Falcon.Authority.Infra.Data.Sqlite.Contexts;
using Owvley.Falcon.Authority.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Owlvey.Falcon.Authority.Infra.Data.Sqlite.Seed
{
    public static class DatabaseSeed
    {
        public static void InitializeDbTestData(this IApplicationBuilder app, IConfiguration configuration)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                //scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                //scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                //scope.ServiceProvider.GetRequiredService<FalconAuthDbContext>().Database.Migrate();

                var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                if (!configurationDbContext.Clients.Any())
                {
                    var webClientId = configuration.GetValue<string>("Authentication:WebClientId");
                    var webClientSecret = configuration.GetValue<string>("Authentication:WebClientSecret");
                    var integrationClientId = configuration.GetValue<string>("Authentication:IntegrationClientId");
                    var integrationClientSecret = configuration.GetValue<string>("Authentication:IntegrationClientSecret");

                    var clients = new Clients(webClientId, webClientSecret, integrationClientId, integrationClientSecret);

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
                
                string[] roles = { "admin", "guest", "integration" };

                foreach (string role in roles)
                {
                    var roleStore = new RoleStore<IdentityRole>(dbContext);

                    if (!dbContext.Roles.Any(r => r.Name == role))
                    {
                        var success = roleStore.CreateAsync(new IdentityRole(role) { NormalizedName = role }).GetAwaiter().GetResult();
                    }
                }

                if (!dbContext.Users.Any())
                {
                    var adminUserName = configuration.GetValue<string>("Authentication:User");
                    var adminPassword = configuration.GetValue<string>("Authentication:Password");
                    var adminEmail = configuration.GetValue<string>("Authentication:Email");

                    if (string.IsNullOrWhiteSpace(adminPassword)) {
                        adminPassword = "P@$$w0rd";
                    }

                    var users = new Users(adminEmail, adminPassword, adminEmail);
                    Console.WriteLine("==========================");
                    Console.WriteLine(adminUserName);
                    Console.WriteLine(adminPassword);
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
                                SecurityStamp = Guid.NewGuid().ToString("D"),
                            };

                            if (identityUser.Email == adminEmail)
                            {
                                var password = new PasswordHasher<User>();
                                var hashed = password.HashPassword(identityUser, adminPassword);
                                identityUser.PasswordHash = hashed;
                            }
                            else {
                                var password = new PasswordHasher<User>();
                                var hashed = password.HashPassword(identityUser, "P@$$w0rd");
                                identityUser.PasswordHash = hashed;
                            }
                            

                            var userStore = new UserStore<User>(dbContext);
                            var result = userStore.CreateAsync(identityUser).GetAwaiter().GetResult();

                            if (testUser.FirstName.Equals("admin", StringComparison.InvariantCultureIgnoreCase))
                            {
                                userStore.AddToRoleAsync(identityUser, "admin").GetAwaiter().GetResult();
                                userStore.AddToRoleAsync(identityUser, "guest").GetAwaiter().GetResult();
                                userStore.AddToRoleAsync(identityUser, "integration").GetAwaiter().GetResult();
                            }

                            if (testUser.FirstName.Equals("guest", StringComparison.InvariantCultureIgnoreCase))
                            {
                                userStore.AddToRoleAsync(identityUser, "guest").GetAwaiter().GetResult();
                            }

                            if (testUser.FirstName.Equals("integration", StringComparison.InvariantCultureIgnoreCase))
                            {
                                userStore.AddToRoleAsync(identityUser, "integration").GetAwaiter().GetResult();
                            }

                            userStore.AddClaimsAsync(identityUser, new List<Claim>() { new Claim("fullname", $"{testUser.FirstName} {testUser.LastName}") }).GetAwaiter().GetResult();

                        }
                    }
                }
            }
        }
    }
}
