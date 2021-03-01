﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using TFW.Framework.Web.Helpers;

namespace System.Web
{
    public static class HttpContext
    {
        private static IHttpContextAccessor _contextAccessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _contextAccessor.HttpContext;

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            if (_contextAccessor != null)
                throw new InvalidOperationException($"Already initialized {nameof(contextAccessor)}");

            _contextAccessor = contextAccessor;
        }
    }
}
