using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.i18n.TimeZone
{
    public interface IThreadTimeZoneProvider
    {
        public TimeZoneInfo TimeZone { get; set; }
    }
}
