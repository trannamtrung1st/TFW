using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using TFW.Framework.Common.Extensions;

namespace TFW.Framework.Http.Helpers
{
    public static class HttpHelper
    {
        public static string[] GetHeaderNames()
        {
            return typeof(HeaderNames).GetAllConstants<string>();
        }

        public static string[] GetMediaTypeNames()
        {
            var applications = typeof(MediaTypeNames.Application).GetAllConstants<string>();
            var images = typeof(MediaTypeNames.Image).GetAllConstants<string>();
            var texts = typeof(MediaTypeNames.Text).GetAllConstants<string>();

            return applications.Concat(images).Concat(texts).ToArray();
        }
    }
}
