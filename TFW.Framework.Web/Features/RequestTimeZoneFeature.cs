using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
