using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank4Us.ServiceApp
{
    public class IdSvrConfig
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Bank4Us.ServiceApp", "Bank4Us Service Application")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "swaggerui",
                    ClientName = "Bank4Us API - Swagger",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AlwaysSendClientClaims = true,
                    RequireConsent = false,
                    RedirectUris =
                    {
                        "https://localhost:44346/swagger/oauth2-redirect.html",
                        "https://localhost:44346/swagger/o2c.html"
                    },
                    AllowedScopes = { "Bank4Us.ServiceApp" }
                },
                new Client
                {
                    ClientId = "angular-client",
                    ClientName = "Angular Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AlwaysSendClientClaims = true,
                    RequireConsent = false,

                    RedirectUris =           { "http://localhost:4200/listcustomer/?callback" },
                    PostLogoutRedirectUris = { "http://localhost:4200" },
                    AllowedCorsOrigins =     { "http://localhost:4200/listcustomer" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "Bank4Us.ServiceApp"
                    }
                }
            };

        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

    }
}
