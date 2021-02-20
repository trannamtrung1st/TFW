using System;

namespace TFW.Framework.WebAPI.Options
{
    public class QueryTimeZoneProviderOptions
    {
        public const string DefaultQueryKey = "tz";

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
