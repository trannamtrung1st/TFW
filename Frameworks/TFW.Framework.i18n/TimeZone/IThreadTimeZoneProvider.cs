using System;

namespace TFW.Framework.i18n.TimeZone
{
    public interface IThreadTimeZoneProvider
    {
        public TimeZoneInfo TimeZone { get; set; }
    }
}
