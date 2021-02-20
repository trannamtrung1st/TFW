using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.WebAPI
{
    public static class RequestThread
    {
        [ThreadStatic]
        private static HttpContext _httpContext;
        public static HttpContext HttpContext
        {
            get
            {
                return _httpContext;
            }
            internal set
            {
                _httpContext = value;
            }
        }
    }
}
