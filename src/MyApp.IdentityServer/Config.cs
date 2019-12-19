using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace MyApp.IdentityServer
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource {
                Name ="myAppApi",
                Scopes = new []{ new Scope("full","full api scope") }
                }
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client
                {
                    ClientId = "myAppClient",
                    ClientName = "Sample client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RedirectUris = { "http://localhost:5001" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "full"
                    },
                    AllowOfflineAccess = true
                },
                new Client
                {
                    AllowAccessTokensViaBrowser = true,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes =  {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "full"
                    },
                    ClientId = "swagger",
                    ClientName = "swagger",
                    ClientSecrets = new[] { new Secret("secret".Sha256()) },
                    RedirectUris = new[] {
                        "http://localhost:5001/swagger/oauth2-redirect.html", // IIS Express
                        "http://localhost:5001/swagger/oauth2-redirect.html", // Kestrel
                    }
                }
        };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "getson",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, "cela.getson@gmail.com"),
                        new Claim(ClaimTypes.Name, "getson")
                    }
                }
            };
        }
    }
}