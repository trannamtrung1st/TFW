using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.Web.Providers;

namespace TFW.Framework.Web.Options
{
    public class RequestTimeZoneOptions
    {
        public const string TimeZoneResponseHeaderName = "Content-TZ";
        public const string DefaultOverrideFallbackQueryKey = "tzfb";
        public const string DefaultOverrideFallbackHeaderName = "Content-TZFallback";
        public const string DefaultOverrideFallbackCookieName = "_tzfb";

        public RequestTimeZoneOptions()
        {
            SupportedTimeZones = new List<TimeZoneInfo>();
            Providers = new List<IRequestTimeZoneProvider>();
        }

        public bool ApplyCurrentTimeZoneToResponseHeaders { get; set; } = true;
        public bool AllowFallback { get; set; }
        public bool AllowOverrideFallback { get; set; } = true;
        public string OverrideFallbackQueryKey { get; set; } = DefaultOverrideFallbackQueryKey;
        public string OverrideFallbackHeaderName { get; set; } = DefaultOverrideFallbackHeaderName;
        public string OverrideFallbackCookieName { get; set; } = DefaultOverrideFallbackCookieName;


        private IList<TimeZoneInfo> _supportedTimeZones;
        public IList<TimeZoneInfo> SupportedTimeZones
        {
            get => _supportedTimeZones;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _supportedTimeZones = value;
            }
        }

        private TimeZoneInfo _defaultRequestTimeZone = TimeZoneInfo.Local;
        public TimeZoneInfo DefaultRequestTimeZone
        {
            get => _defaultRequestTimeZone; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _defaultRequestTimeZone = value;
            }
        }

        private IList<IRequestTimeZoneProvider> _providers;
        public IList<IRequestTimeZoneProvider> Providers
        {
            get => _providers;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _providers = value;
            }
        }

        public RequestTimeZoneOptions AddTimeZone(TimeZoneInfo timeZoneInfo)
        {
            SupportedTimeZones.Add(timeZoneInfo);
            return this;
        }

        public RequestTimeZoneOptions AddProvider(IRequestTimeZoneProvider timeZoneProvider)
        {
            Providers.Add(timeZoneProvider);
            return this;
        }
    }
}
