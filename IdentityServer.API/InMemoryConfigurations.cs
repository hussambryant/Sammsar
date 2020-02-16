using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer.API
{
    public class InMemoryConfigurations
    {
        public static List<ApiResource> ApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "xxx", DisplayName = "Xxx",
                    Scopes = new List<Scope>{ new Scope("xxx")}
                }
            };
        }
        public static List<Client> Clients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "xxx",
                    AllowOfflineAccess = true,
                    ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new[] {"xxx", IdentityServerConstants.StandardScopes.OfflineAccess},
                    //RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    //RefreshTokenExpiration = TokenExpiration.Sliding,
                    //SlidingRefreshTokenLifetime = int.MaxValue,
                    //UpdateAccessTokenClaimsOnRefresh = true,

                }
            };
        }
    }
}