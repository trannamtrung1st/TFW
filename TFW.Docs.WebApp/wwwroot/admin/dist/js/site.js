const tokenInfoKey = 'tokenInfo';

function clearToken() {
  localStorage.removeItem(tokenInfoKey);
}

function checkToken() {
  const tokenInfo = localStorage[tokenInfoKey] ? JSON.stringify(localStorage[tokenInfoKey]) : null;

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
            url: "@apiUrl/@ApiEndpoint.USER_API/login",
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
              location.href = '@Routing.LOGOUT';
            }
          });
        }
      }, minRefDiff * 60 * 1000);
    } else
      setTimeout(() => {
        location.href = '@Routing.LOGOUT';
      }, minDiff * 60 * 1000);
  }

  function saveToken(data) {
    localStorage.setItem(tokenInfoKey, JSON.stringify(data));
  }
}
