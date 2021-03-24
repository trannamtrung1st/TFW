using System;

namespace TFW.Framework.Web.Options
{
    public class HeaderClientTimeZoneProviderOptions
    {
        public const string DefaultHeaderName = "Content-TZO";

        private string _headerName = DefaultHeaderName;
        public string HeaderName
        {
            get => _headerName; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _headerName = value;
            }
        }
    }
}
