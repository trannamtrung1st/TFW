using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using TFW.Framework.WebAPI.Helpers;

namespace System.Web
{
    public static class HttpContext
    {
        private static IHttpContextAccessor _contextAccessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _contextAccessor.HttpContext;

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public static T GetItem<T>(string key)
        {
            return Current.GetItem<T>(key);
        }

        public static T GetRequiredService<T>()
        {
            return Current.GetRequiredService<T>();
        }
    }
}
