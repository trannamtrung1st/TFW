using System;

namespace TFW.Framework.Web.Features
{
    public interface IRequestTimeZoneFeature
    {
        TimeZoneInfo ClientTimeZone { get; set; }
    }

    public class RequestTimeZoneFeature : IRequestTimeZoneFeature
    {
        public TimeZoneInfo ClientTimeZone { get; set; }
    }
}
