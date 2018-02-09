using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingBlocks.IdentityServer4.RavenDB.Stores;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Identity.Application.Setup
{
    public static class BaseConfig
    {
        public static IEnumerable<ApiResource> GetApis() => new[]
        {
            new ApiResource("echoapi", "EchoService")
        };

        public static IEnumerable<IdentityResource> GetResources() => new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<Client> GetClients(
            IDictionary<string, string> clientsUrl
        ) => new[]
        {
            new Client
            {
                ClientId = "echoapiswaggerui",
                ClientName = "Echo Api Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = { $"{clientsUrl["EchoApiUrl"]}/swagger/o2c.html" },
                PostLogoutRedirectUris = { $"{clientsUrl["EchoApiUrl"]}/swagger/" },

                AllowedScopes =
                {
                    "echoapi"
                },
                RequireConsent = false
            },

            new Client
            {
                ClientId = "echoweb",
                ClientName = "EchoWeb",
                ClientSecrets = new List<Secret>
                {
                    new Secret("secret".Sha256())
                },
                ClientUri = $"{clientsUrl["EchoWebUrl"]}",                             // public uri of the client
                AllowedGrantTypes = GrantTypes.Hybrid,
                AllowAccessTokensViaBrowser = false,
                RequireConsent = false,
                AllowOfflineAccess = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                RedirectUris = new List<string>
                {
                    $"{clientsUrl["EchoWebUrl"]}/signin-oidc"
                },
                PostLogoutRedirectUris = new List<string>
                {
                    $"{clientsUrl["EchoWebUrl"]}/signout-callback-oidc"
                },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "echoapi",
                }
                
            }
        };

        public static async Task LoadSeed(
            IRavenDBClientStore clientStore, 
            IRavenDBResourceStore resourceStore,
            IConfiguration configuration
            )
        {
            if (!await resourceStore.HasStoredApis())
            {
                foreach (var api in GetApis())
                {
                    await resourceStore.StoreAsync(api);
                }
            }

            if (!await resourceStore.HasStoredIdentities())
            {
                foreach (var identity in GetResources())
                {
                    await resourceStore.StoreAsync(identity);
                }
            }

            if (!await clientStore.HasStoredClients())
            {
                var urls = new Dictionary<string, string>
                {
                    {"EchoApiUrl", configuration["EchoApiUrl"]},
                    {"EchoWebUrl", configuration["EchoWebUrl"]}
                };

                foreach (var client in GetClients(urls))
                {
                    await clientStore.StoreAsync(client);
                }
            }
        }

    }
}
