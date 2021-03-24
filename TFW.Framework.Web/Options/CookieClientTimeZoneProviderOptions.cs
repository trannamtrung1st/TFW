using System;

namespace TFW.Framework.Web.Options
{
    public class CookieClientTimeZoneProviderOptions
    {
        public const string DefaultCookieName = "_tzo";

        private string _cookieName = DefaultCookieName;
        public string CookieName
        {
            get => _cookieName; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _cookieName = value;
            }
        }
    }
}
