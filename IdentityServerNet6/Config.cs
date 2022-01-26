﻿using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerNet6
{
    public class Config
    {
        internal static class ResourceManager
        {
            public static IEnumerable<ApiResource> ApiResources =>
                new List<ApiResource>
                {
                    new ApiResource
                    {
                        Name = "identity_service_api",
                        DisplayName = "Identity Service",
                        Scopes = new List<string> { "identity.api.full", "identity.api.read", "identity.api.write"},
                    },
                    new ApiResource
                    {
                        Name = "catalog_service_api",
                        DisplayName = "Catalog Service",
                        Scopes = new List<string> { "catalog.api.full", "catalog.api.read", "catalog.api.write"},
                    },
                };
        }

        internal static class ScopeManager
        {
            public static IEnumerable<ApiScope> ApiScopes =>
                new List<ApiScope>
                {
                    new ApiScope
                    {
                        Name = "identity.api.read",
                        DisplayName = "Read Scope",
                    },
                    new ApiScope
                    {
                        Name = "identity.api.write",
                        DisplayName = "Write Scope",
                    },
                    new ApiScope
                    {
                        Name = "identity.api.full",
                        DisplayName = "Full Scope",
                    },
                    new ApiScope
                    {
                        Name = "catalog.api.read",
                        DisplayName = "Read Scope",
                    },
                    new ApiScope
                    {
                        Name = "catalog.api.write",
                        DisplayName = "Write Scope",
                    },
                    new ApiScope
                    {
                        Name = "catalog.api.full",
                        DisplayName = "Full Scope",
                    }
                };
        }

        internal static class ClientManager
        {
            public static IEnumerable<Client> Clients =>
                new List<Client>
                {
                    new Client
                    {
                         ClientName = "IdentityServiceClient",
                         ClientId = "identity_service_client_full",
                         AllowedGrantTypes = GrantTypes.ClientCredentials,
                         ClientSecrets = { new Secret("secret1".Sha256()) },
                         AllowedScopes = new List<string> { "identity.api.full"},
                    },
                    new Client
                    {
                         ClientName = "CatalogServiceClient",
                         ClientId = "catalog_service_client_full",
                         AllowedGrantTypes = GrantTypes.ClientCredentials,
                         ClientSecrets = { new Secret("secret2".Sha256()) },
                         AllowedScopes = new List<string> { IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, "catalog.api.full", "catalog.api.read", "catalog.api.write"},
                         AlwaysSendClientClaims = true,
                         AlwaysIncludeUserClaimsInIdToken = true
                    }
                };
        }
    }
}
