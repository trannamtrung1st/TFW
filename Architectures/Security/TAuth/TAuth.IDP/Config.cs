// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace TAuth.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource("roles", "User role(s)", new string[]
                {
                    JwtClaimTypes.Role
                })
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            { };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client()
                {
                    ClientName = "Resource Client",
                    ClientId = "resource-client-id",
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new[]
                    {
                        "https://localhost:44385/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44385/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles"
                    },
                    ClientSecrets =
                    {
                        new Secret("resource-client-secret".Sha256())
                    },
                    RequirePkce = true,
                    RequireConsent = true
                },
                new Client
                {
                    ClientId = "resource-client-js-id",
                    ClientName = "Resource Client JS",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris =
                    {
                        "http://localhost:52330/callback.html"
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:52330/index.html"
                    },
                    AllowedCorsOrigins =
                    {
                        "http://localhost:52330"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles"
                    },
                    RequireConsent = true
                }
            };
    }
}