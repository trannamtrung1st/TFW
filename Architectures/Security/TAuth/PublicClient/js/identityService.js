const IdentityService = () => {
    const config = {
        authority: "https://localhost:5001/",
        client_id: "resource-client-js-id",
        redirect_uri: "http://localhost:52330/callback.html",
        response_type: "id_token token",
        scope: "openid profile address roles",
        post_logout_redirect_uri: "http://localhost:52330/index.html",
    };
    const manager = new Oidc.UserManager(config);

    const getUser = (handler) => {
        manager.getUser().then(handler);
    }

    const login = () => {
        manager.signinRedirect();
    }

    const logout = () => {
        manager.signoutRedirect();
    }

    const authenticateUser = () => {
        manager.getUser().then(user => {
            if (!user) {
                login();
            }
        });
    }

    return {
        getUser,
        login,
        logout,
        authenticateUser
    }
};