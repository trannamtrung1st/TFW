const IdentityService = () => {
    const config = {
        authority: "https://localhost:5001/",
        client_id: "resource-client-js-id",
        redirect_uri: "http://localhost:52330/callback.html",
        response_type: "id_token token",
        scope: "openid profile address roles resource_api.full",
        post_logout_redirect_uri: "http://localhost:52330/index.html",
    };
    const manager = new Oidc.UserManager(config);
    let _token = null;

    const getUser = (handler) => {
        manager.getUser().then(handler);
    }

    const login = () => {
        manager.signinRedirect();
    }

    const logout = () => {
        _token = null;
        manager.signoutRedirect();
    }

    const authenticateUser = () => {
        manager.getUser().then(user => {
            if (!user) {
                login();
            } else {
                _token = user.access_token;
            }
        });
    }

    const accessToken = () => {
        return _token;
    };

    return {
        getUser,
        login,
        logout,
        authenticateUser,
        accessToken
    }
};