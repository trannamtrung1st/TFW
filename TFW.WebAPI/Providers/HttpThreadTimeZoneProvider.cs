using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFW.Framework.i18n.TimeZone;
using TFW.Framework.Web.Features;

namespace TFW.WebAPI.Providers
{
    public class HttpThreadTimeZoneProvider : IThreadTimeZoneProvider
    {
        public TimeZoneInfo TimeZone
        {
            get => HttpContext.Current.Features.Get<IRequestTimeZoneFeature>()?.ClientTimeZone;
            set => HttpContext.Current.Features.Set<IRequestTimeZoneFeature>(new RequestTimeZoneFeature
            {
                ClientTimeZone = value
            });
        }
    }
}
