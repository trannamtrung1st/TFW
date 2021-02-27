using System;

namespace TFW.Framework.Web.Options
{
    public class CookieTimeZoneProviderOptions
    {
        public const string DefaultCookieName = "_tz";

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
