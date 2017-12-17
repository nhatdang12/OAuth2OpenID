using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer4WithAspNetIdentity
{
    public class Config
    {
        public static Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }

        // scopes define identity data in your system (for OIDC)
        // OpenId (user id) and Profile (first name, last name etc..)
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        // scopes define the API resources in your system (for OAuth2)
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
            // client credentials client
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },
            // client mvc client
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    
                    //RedirectUris           = { "http://localhost:5002/signin-oidc" },
                    //PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    RedirectUris           = { Configuration[Constants.MVCCLIENT_HOSTURL] + "/signin-oidc" },
                    PostLogoutRedirectUris = { Configuration[Constants.MVCCLIENT_HOSTURL] + "/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },
                    AllowOfflineAccess = true
                },
                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris =           { Configuration[Constants.JSCLIENT_HOSTURL] + "/callback.html" },
                    PostLogoutRedirectUris = { Configuration[Constants.JSCLIENT_HOSTURL] + "/index.html" },
                    AllowedCorsOrigins =     { Configuration[Constants.JSCLIENT_HOSTURL] },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                }
            };
        }
    }
}
