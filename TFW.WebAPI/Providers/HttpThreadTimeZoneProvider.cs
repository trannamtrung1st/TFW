using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TFW.Framework.i18n;

namespace TFW.WebAPI.Providers
{
    public class HttpThreadTimeZoneProvider : IThreadTimeZoneProvider
    {
        public const string TimeZoneKey = nameof(Time.ThreadTimeZone);

        public TimeZoneInfo TimeZone
        {
            get => HttpContext.Current.Items[TimeZoneKey] as TimeZoneInfo;
            set => HttpContext.Current.Items[TimeZoneKey] = value;
        }
    }
}
