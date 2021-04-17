﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Helpers
{
    public static class HttpContextExtensions
    {
        public static T GetItem<T>(this HttpContext context, string key)
        {
            if (context?.Items.ContainsKey(key) != true)
                throw new KeyNotFoundException(key);

            return (T)context.Items[key];
        }
    }
}
