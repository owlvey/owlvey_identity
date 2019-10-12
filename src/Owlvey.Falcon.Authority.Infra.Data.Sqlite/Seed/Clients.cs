using IdentityServer4;
using IdentityServer4.Models;
using Owvley.Falcon.Authority.Domain.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Owlvey.Falcon.Authority.Infra.Data.Sqlite.Seed
{
    internal class Clients
    {
        /// <summary>
        /// App can access to IS
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> Get()
        {
            return new List<Client> {
                new Client {
                    ClientId = "B0D76E84BF394F1297CABBD7337D42B9",
                     ClientSecrets = new List<Secret> { new Secret("0da45603-282a-4fa6-a20b-2d4c3f2a2127".Sha256()) },
                    ClientName = "Passwords Client",
                    AllowedGrantTypes = IdentityServer4.Models.GrantTypes.Code,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api",
                        "api.auth"
                    },
                    RequireConsent = false
                }
            };
        }
    }

    internal class Resources
    {
        /// <summary>
        /// Identity resources are data like user ID, name, or email address of a user. 
        /// An identity resource has a unique name, and you can assign arbitrary claim types to it. 
        /// These claims will then be included in the identity token for the user. 
        /// The client will use the scope parameter to request access to an identity resource.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        /// <summary>
        /// To allow clients to request access tokens for APIs, you need to define API resources
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> {
                new ApiResource {
                    
                    Name = "api",
                    DisplayName = "api",
                    Description = "api",
                    Scopes = new List<Scope> {
                        new Scope("role"),
                        new Scope("name"),
                        new Scope("fullname"),
                        new Scope("email"),
                    }
                },
                //new ApiResource {
                //    Name = "api.auth",
                //    DisplayName = "api.auth",
                //    Description = "api.auth",
                //    Scopes = new List<Scope> {
                //        new Scope("role"),
                //        new Scope("name"),
                //        new Scope("fullname"),
                //        new Scope("email"),
                //    }
                //}
            };
        }
    }

    internal class Users
    {
        public static List<User> Get()
        {
            return new List<User> {
                new User {
                    UserName = "admin@owlvey.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@owlvey.com",
                },
                new User {
                   UserName = "guest@owlvey.com",
                    FirstName = "Guest",
                    LastName = "Guest",
                    Email = "guest@owlvey.com",
                 }
            };
        }
    }
}
