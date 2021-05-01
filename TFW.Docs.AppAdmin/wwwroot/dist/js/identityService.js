const IdentityService = ({
  requestTokenEndpoint,
  initUserEndpoint,
  logOutPage,
  logInPage
}) => {
  const tokenInfoKey = 'tokenInfo';
  const saveToken = (model) => {
    localStorage.setItem(tokenInfoKey, JSON.stringify(model));
  };
  const login = ({ formData, success = null, error = null, complete = null }) => {
    $.ajax({
      url: requestTokenEndpoint,
      type: 'post',
      contentType: false,
      processData: false,
      cache: false,
      data: formData,
      success: (data) => {
        data.issuedAt = new Date();
        saveToken(data);
        if (success)
          success(data);
      },
      error: error,
      complete: complete
    });
  };
  const logOut = () => {
    location.href = logOutPage;
  };
  const navigateToLoginPage = (returnUrl = location.pathname) => {
    const loginUrl = new URL(logInPage, location.origin);
    if (returnUrl) {
      loginUrl.searchParams.set('returnUrl', returnUrl);
    }
    location.href = loginUrl.href;
  };
  const validateToken = () => {
    const tokenInfoCache = localStorage[tokenInfoKey];
    const tokenInfo = tokenInfoCache ? JSON.stringify(tokenInfoCache) : null;

    if (tokenInfo?.expires_in) {
      const curAccessToken = tokenInfo.access_token;
      const issuedAt = tokenInfo.issuedAt;
      const cur = moment(new Date());
      const exp = moment(issuedAt).add(parseInt(curExpIn), 'seconds');

      let minDiff = exp.diff(cur, 'minutes');
      minDiff = minDiff < 0 ? 0 : minDiff;
      let minRefDiff = minDiff - 5;
      minRefDiff = minRefDiff < 0 ? 0 : minRefDiff;

      console.log('refresh token in ' + minRefDiff + ' mins');

      if (tokenInfo.refresh_token) {
        setTimeout(() => {
          if (tokenInfo.access_token == curAccessToken) {
            const formData = new FormData();
            formData.append('grant_type', 'refresh_token');
            formData.append('refresh_token', tokenInfo.refresh_token);
            login({
              formData,
              success: (data) => {
                validateToken();
                if (minRefDiff == 0)
                  location.reload();
              },
              error: (e) => {
                logOut();
              }
            });
          }
        }, minRefDiff * 60 * 1000);
      } else {
        setTimeout(logOut, minDiff * 60 * 1000);
      }
    }
  };
  return {
    saveToken,
    logOut,
    navigateToLoginPage,
    validateToken,
    login,

    authorize: () => {
      const tokenInfoCache = localStorage[tokenInfoKey];
      const tokenInfo = tokenInfoCache ? JSON.stringify(tokenInfoCache) : null;

      if (!tokenInfo) {
        return navigateToLoginPage();
      }

      if (tokenInfo) {
        validateToken();
      }
    },

    clearToken: () => {
      localStorage.removeItem(tokenInfoKey);
    },

    initializeUser: ({ formData, success, error, complete }) => {
      $.ajax({
        url: initUserEndpoint,
        type: 'post',
        contentType: false,
        processData: false,
        cache: false,
        data: formData,
        success: success,
        error: error,
        complete: complete
      });
    },
  };
}
