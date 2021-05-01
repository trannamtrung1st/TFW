const IdentityService = ({
  requestTokenEndpoint,
  initUserEndpoint,
  logInPage
}) => {
  let lastTokenValidationHandler;
  const tokenInfoKey = 'tokenInfo';
  const getTokenStorage = (persistent = null) => {
    if (persistent === true) return localStorage;
    else if (persistent === false) return sessionStorage;
    else if (localStorage[tokenInfoKey]) return localStorage;
    return sessionStorage;
  };
  const saveToken = (model, rememberMe) => {
    getTokenStorage(rememberMe).setItem(tokenInfoKey, JSON.stringify(model));
  };
  const login = ({ formData, success = null, error = null, complete = null, rememberMe = null }) => {
    formData.append('grant_type', 'password');
    $.ajax({
      url: requestTokenEndpoint,
      type: 'post',
      contentType: false,
      processData: false,
      cache: false,
      data: formData,
      success: (data) => {
        data.issuedAt = new Date();
        saveToken(data, rememberMe);
        if (success)
          success(data);
      },
      error: error,
      complete: complete
    });
  };
  const logOut = () => {
    getTokenStorage().removeItem(tokenInfoKey);
    navigateToLoginPage();
  };
  const navigateToLoginPage = (returnUrl = location.pathname) => {
    const loginUrl = new URL(logInPage, location.origin);
    if (returnUrl) {
      loginUrl.searchParams.set('returnUrl', returnUrl);
    }
    location.href = loginUrl.href;
  };
  const validateToken = () => {
    const tokenInfoCache = getTokenStorage()[tokenInfoKey];
    const tokenInfo = tokenInfoCache ? JSON.parse(tokenInfoCache) : null;

    if (tokenInfo?.expires_in) {
      const curAccessToken = tokenInfo.access_token;
      const curExpIn = tokenInfo.expires_in;
      const issuedAt = tokenInfo.issuedAt;
      const cur = moment(new Date());
      const exp = moment(issuedAt).add(parseInt(curExpIn), 'seconds');

      let minDiff = exp.diff(cur, 'minutes');
      minDiff = minDiff < 0 ? 0 : minDiff;
      let minRefDiff = minDiff - 5;
      minRefDiff = minRefDiff < 0 ? 0 : minRefDiff;

      console.log('refresh token in ' + minRefDiff + ' mins');

      if (lastTokenValidationHandler) clearTimeout(lastTokenValidationHandler);

      if (tokenInfo.refresh_token) {
        lastTokenValidationHandler = setTimeout(() => {
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
                console.log(e);
                logOut();
              }
            });
          }
        }, minRefDiff * 60 * 1000);
      } else {
        lastTokenValidationHandler = setTimeout(logOut, minDiff * 60 * 1000);
      }
    }
  };
  const isTokenValid = () => {
    const tokenInfoCache = getTokenStorage()[tokenInfoKey];
    const tokenInfo = tokenInfoCache ? JSON.parse(tokenInfoCache) : null;

    if (!tokenInfo) return false;

    if (tokenInfo?.expires_in) {
      const curExpIn = tokenInfo.expires_in;
      const issuedAt = tokenInfo.issuedAt;
      const cur = moment(new Date());
      const exp = moment(issuedAt).add(parseInt(curExpIn), 'seconds');

      return exp.isAfter(cur);
    }

    return true;
  };
  return {
    saveToken,
    logOut,
    navigateToLoginPage,
    validateToken,
    login,
    getTokenStorage,
    isTokenValid,

    authorize: () => {
      const tokenInfoCache = getTokenStorage()[tokenInfoKey];
      const tokenInfo = tokenInfoCache ? JSON.parse(tokenInfoCache) : null;

      if (!tokenInfo) {
        return navigateToLoginPage();
      }

      if (tokenInfo) {
        validateToken();
      }
    },

    clearToken: () => {
      getTokenStorage().removeItem(tokenInfoKey);
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
