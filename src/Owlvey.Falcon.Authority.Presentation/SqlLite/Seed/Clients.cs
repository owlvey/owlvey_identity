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
        private static List<Client> clients = new List<Client>();

        static Clients()
        {

        }

        public Clients(string webClientId, string webClientSecret, string integrationClientId, string integrationClientSecret)
        {
            clients = new List<Client> {
                new Client {
                    ClientId = webClientId,
                    ClientSecrets = new List<Secret> {
                        new Secret(webClientSecret.Sha256()) },
                    ClientName = "Passwords Client",
                    AllowedGrantTypes = IdentityServer4.Models.GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api",
                        "api.auth"
                    },
                    RequireConsent = false
                },
                new Client {
                    ClientId = integrationClientId,
                    ClientSecrets = new List<Secret> { new Secret(integrationClientSecret.Sha256()) },
                    ClientName = "Default Client",
                    AllowedGrantTypes = IdentityServer4.Models.GrantTypes.ClientCredentials,
                    AllowedScopes = new List<string>
                    {
                        "api",
                    },

                    RequireClientSecret = true,
                    RequireConsent = false,
                    ClientClaimsPrefix = "",
                    Claims = new  List<Claim>(){
                        new Claim("sub", integrationClientId),
                        new Claim("name", "Integration"),
                        new Claim("fullname", "Integration"),
                        new Claim("role", "integration"),
                         new Claim("role", "admin")
                    }
                }
            };
        }

        /// <summary>
        /// App can access to IS
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> Get()
        {
            return clients;
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
                    Scopes  = new List<Scope> {
                        new Scope("api")
                    },
                    UserClaims = new List<string>
                    {
                        "role",
                        "name",
                        "fullname",
                        "givenname",
                        "email"
                    }
                }
            };
        }
    }

    internal class Users
    {

        private static List<User> users = new List<User>();

        static Users()
        {
            
        }

        public Users(string adminUserName, string adminPassword, string adminEmail)
        {
            users = new List<User> {
                new User {
                    UserName = adminUserName,
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = adminEmail,
                },
                new User {
                   UserName = "guest@owlvey.com",
                    FirstName = "Guest",
                    LastName = "Guest",
                    Email = "guest@owlvey.com",
                 },
                new User {
                   UserName = "integration@owlvey.com",
                    FirstName = "Integration",
                    LastName = "Integration",
                    Email = "integration@owlvey.com",
                 }
            };
        }

        public static List<User> Get()
        {
            return users;
        }
    }
}
