﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace TAuth.IDP
{
    public static class Config
    {
        private static readonly IdentityResource CustomEmailResource;

        static Config()
        {
            var standardEmailResource = new IdentityResources.Email();
            CustomEmailResource = new IdentityResource(standardEmailResource.Name, standardEmailResource.DisplayName, standardEmailResource.UserClaims);
            CustomEmailResource.UserClaims.Add(JwtClaimTypes.EmailVerified);
        }

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                CustomEmailResource,
                new IdentityResource("roles", "User role(s)", new string[]
                {
                    JwtClaimTypes.Role
                })
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("resource_api", "Resource API"/*, new[]
                {
                    JwtClaimTypes.Role
                }*/)
                {
                    Scopes = { "resource_api.full", "resource_api.background" },
                    ApiSecrets =
                    {
                        new Secret("resource-api-secret".Sha256())
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("resource_api.full", "Full access to Resource API", new[]
                {
                    JwtClaimTypes.Role
                }),
                new ApiScope("resource_api.background", "Background access to Resource API")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client()
                {
                    AllowOfflineAccess = true,
                    //RefreshTokenExpiration = TokenExpiration.Sliding,
                    //SlidingRefreshTokenLifetime = ...,
                    UpdateAccessTokenClaimsOnRefresh = true,
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
                        IdentityServerConstants.StandardScopes.Email,
                        //IdentityServerConstants.StandardScopes.OfflineAccess,
                        "roles",
                        "resource_api.full"
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
                        "http://localhost:52330/callback.html",
                        "http://localhost:52330/silent-refresh.html"
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
                        IdentityServerConstants.StandardScopes.Email,
                        "roles",
                        "resource_api.full"
                    },
                    RequireConsent = true
                },
                new Client()
                {
                    AccessTokenType = AccessTokenType.Reference,
                    AccessTokenLifetime = 600,
                    //RefreshTokenExpiration = TokenExpiration.Sliding,
                    //SlidingRefreshTokenLifetime = ...,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    ClientName = "Worker Client",
                    ClientId = "worker-client-id",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes =
                    {
                        "resource_api.background"
                    },
                    ClientSecrets =
                    {
                        new Secret("worker-client-secret".Sha256())
                    }
                },
            };
    }

    public class AppSettings
    {
        public bool UseAuthenticatorApp { get; set; }
    }
}