using System;
using IdentityServer4;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;

namespace Owlvey.Falcon.Authority.Presentation
{
    public static class Config
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
                    ClientSecrets = new List<Secret> {
                        new Secret("0da45603-282a-4fa6-a20b-2d4c3f2a2127".Sha256()) },
                    ClientName = "Passwords Client",
                    AllowedGrantTypes = IdentityServer4.Models.GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api",
                    },
                    RequireConsent = false
                },
                new Client {
                    ClientId = "CF4A9ED44148438A99919FF285D8B48D",
                    ClientSecrets = new List<Secret> { new Secret("0da45603-282a-4fa6-a20b-2d4c3f2a2127".Sha256()) },
                    ClientName = "Default Client",
                    AllowedGrantTypes = IdentityServer4.Models.GrantTypes.ClientCredentials,
                    AllowedScopes = new List<string>
                    {
                        "api",
                    },
                    RequireClientSecret = true,
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
            public static List<IdentityUser> Get()
            {
                return new List<IdentityUser> {
                    new IdentityUser {
                        UserName = "admin@owlvey.com",
                        Email = "admin@owlvey.com",
                    },
                    new IdentityUser {
                       UserName = "guest@owlvey.com",
                       Email = "guest@owlvey.com",
                     },
                    new IdentityUser {
                        UserName = "integration@owlvey.com",
                        Email = "integration@owlvey.com",
                     }
                };
            }
        }
    }
}
