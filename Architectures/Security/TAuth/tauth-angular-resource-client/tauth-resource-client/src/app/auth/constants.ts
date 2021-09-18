import { OidcClientSettings } from "oidc-client";

import { environment } from "@environments/environment";

// OIDC
const origin = window.location.origin;
export const ODIC_CONFIG: OidcClientSettings = {
    authority: environment.idpUrl,
    client_id: "resource-client-js-id",
    redirect_uri: `${origin}/callback`,
    response_type: "code",
    scope: "openid profile address roles resource_api.full",
    post_logout_redirect_uri: origin,
    response_mode: "query" // code + PKCE
};

// ROLES
export const ROLES = {
    Administrator: 'Administrator'
};