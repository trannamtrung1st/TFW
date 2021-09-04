using System;
using System.Collections.Generic;
using System.Linq;

namespace TFW.Framework.i18n.Helpers
{
    public static class TimeZoneHelper
    {
        public static TimeZoneInfo FindById(string timeZoneId)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }

        public static bool TryFindById(string timeZoneId, out TimeZoneInfo timeZoneInfo)
        {
            try
            {
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);

                timeZoneInfo = null;

                return false;
            }
        }

        public static IReadOnlyList<TimeZoneInfo> GetAllTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones();
        }

        public static TimeZoneInfo GetFirstTimeZoneByUTCOffset(TimeSpan timeSpan)
        {
            var isPositive = timeSpan.TotalMinutes >= 0;

            return TimeZoneInfo.GetSystemTimeZones()
                .Select(o => new
                {
                    TimeZoneInfo = o,
                    DiffMinutes = (o.BaseUtcOffset - timeSpan).TotalMinutes
                })
                .Where(o => o.DiffMinutes >= 0 == isPositive)
                .OrderBy(o => o.DiffMinutes)
                .Select(o => o.TimeZoneInfo)
                .FirstOrDefault();
        }
    }
}
