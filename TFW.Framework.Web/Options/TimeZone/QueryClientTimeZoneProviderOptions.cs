using System;

namespace TFW.Framework.Web.Options
{
    public class QueryClientTimeZoneProviderOptions
    {
        public const string DefaultQueryKey = "tzo";

        private string _queryKey = DefaultQueryKey;
        public string QueryKey
        {
            get => _queryKey; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _queryKey = value;
            }
        }
    }
}
