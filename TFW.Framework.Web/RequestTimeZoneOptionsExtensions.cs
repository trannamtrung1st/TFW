using TFW.Framework.Web.Options;
using TFW.Framework.Web.Providers;

namespace TFW.Framework.Web
{
    public static class RequestTimeZoneOptionsExtensions
    {
        public static RequestTimeZoneOptions AddQuery(this RequestTimeZoneOptions options)
        {
            return options.AddProvider(new QueryTimeZoneProvider());
        }

        public static RequestTimeZoneOptions AddQueryClient(this RequestTimeZoneOptions options)
        {
            return options.AddProvider(new QueryClientTimeZoneProvider());
        }

        public static RequestTimeZoneOptions AddCookie(this RequestTimeZoneOptions options)
        {
            return options.AddProvider(new CookieTimeZoneProvider());
        }

        public static RequestTimeZoneOptions AddCookieClient(this RequestTimeZoneOptions options)
        {
            return options.AddProvider(new CookieClientTimeZoneProvider());
        }

        public static RequestTimeZoneOptions AddHeader(this RequestTimeZoneOptions options)
        {
            return options.AddProvider(new HeaderTimeZoneProvider());
        }

        public static RequestTimeZoneOptions AddHeaderClient(this RequestTimeZoneOptions options)
        {
            return options.AddProvider(new HeaderClientTimeZoneProvider());
        }

    }
}
