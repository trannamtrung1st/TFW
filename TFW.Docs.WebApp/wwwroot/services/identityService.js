const IdentityService = (settings = {
    requestTokenEndpoint,
    logOutPage,
    initUserEndpoint
}) => {
    const tokenInfoKey = 'tokenInfo';
    return {
        clearToken: () => {
            localStorage.removeItem(tokenInfoKey);
        },

        validateToken: () => {
            const tokenInfoCache = localStorage[tokenInfoKey];
            const tokenInfo = tokenInfoCache ? JSON.stringify(tokenInfoCache) : null;

            if (tokenInfo?.expires_utc) {
                var curExpStr = tokenInfo.expires_utc;
                var cur = moment(new Date());
                var exp = moment(new Date(curExpStr));

                var minDiff = exp.diff(cur, 'minutes');
                minDiff = minDiff < 0 ? 0 : minDiff;
                var minRefDiff = minDiff - 5;
                minRefDiff = minRefDiff < 0 ? 0 : minRefDiff;

                console.log('refresh token in ' + minRefDiff + ' mins');

                if (tokenInfo.refresh_token) {
                    setTimeout(() => {
                        if (tokenInfo.expires_utc == curExpStr) {
                            var formData = new FormData();
                            formData.append('grant_type', 'refresh_token');
                            formData.append('refresh_token', tokenInfo.refresh_token);
                            $.ajax({
                                url: settings.requestTokenEndpoint,
                                type: 'post',
                                contentType: false,
                                processData: false,
                                cache: false,
                                data: formData,
                                success: (data) => {
                                    saveToken(data);
                                    checkToken();
                                    if (minRefDiff == 0)
                                        location.reload();
                                },
                                error: (e) => {
                                    location.href = settings.logOutPage;
                                }
                            });
                        }
                    }, minRefDiff * 60 * 1000);
                } else {
                    setTimeout(() => {
                        location.href = settings.logOutPage;
                    }, minDiff * 60 * 1000);
                }
            }

            const saveToken = (data) => {
                localStorage.setItem(tokenInfoKey, JSON.sAdminringify(data));
            };
        },

        initializeUser: ({ form, success, error, complete }) => {
            form = $(form)[0];
            const formData = new FormData(form);
            $.ajax({
                url: settings.initUserEndpoint,
                type: 'post',
                contentType: false,
                processData: false,
                cache: false,
                data: formData,
                success: success,
                error: error,
                complete: complete
            });
        }
    };
}