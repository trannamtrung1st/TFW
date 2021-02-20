using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TFW.Framework.i18n.Helpers
{
    public static class DateTimeHelper
    {
        public static bool TryConvertToDateTime(this string str, string dateFormat, out DateTime dateTime,
            IFormatProvider formatProvider = null,
            DateTimeStyles dateTimeStyles = DateTimeStyles.None)
        {
            return DateTime.TryParseExact(s: str,
                    format: dateFormat,
                    provider: formatProvider ?? CultureInfo.InvariantCulture,
                    style: dateTimeStyles, out dateTime);
        }

        public static bool TryConvertToDateTime(this string str, string[] dateFormats, out DateTime dateTime,
            IFormatProvider formatProvider = null,
            DateTimeStyles dateTimeStyles = DateTimeStyles.None)
        {
            return DateTime.TryParseExact(s: str,
                    formats: dateFormats,
                    provider: formatProvider ?? CultureInfo.InvariantCulture,
                    style: dateTimeStyles, out dateTime);
        }

        public static bool TryConvertToTimeSpan(this string str, out TimeSpan timeSpan)
        {
            return TimeSpan.TryParse(str, out timeSpan);
        }

        public static DateTime ToTimeZone(this DateTime dateTime, TimeZoneInfo destTimeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTime, destTimeZone);
        }

        public static DateTime ToTimeZone(this DateTime dateTime, string destTimeZoneId)
        {
            TimeZoneInfo destTimeZone = TimeZoneInfo.FindSystemTimeZoneById(destTimeZoneId);
            return TimeZoneInfo.ConvertTime(dateTime, destTimeZone);
        }

        public static DateTime ToTimeZoneFromUtc(this DateTime utcDate, string timeZoneId)
        {
            TimeZoneInfo sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDate, sourceTimeZone);
            return dateTime;
        }

        public static DateTime ToTimeZoneFromUtc(this DateTime utcDate, TimeZoneInfo timeZoneInfo)
        {
            DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDate, timeZoneInfo);
            return dateTime;
        }

        public static DateTimeOffset ToTimeZone(this DateTimeOffset utcOffset, string timeZoneId)
        {
            TimeZoneInfo sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var offset = TimeZoneInfo.ConvertTime(utcOffset, sourceTimeZone);
            return offset;
        }

        public static DateTimeOffset ToTimeZone(this DateTimeOffset utcOffset, TimeZoneInfo timeZoneInfo)
        {
            var offset = TimeZoneInfo.ConvertTime(utcOffset, timeZoneInfo);
            return offset;
        }

        public static DateTime ToUtcFromTimeZone(this DateTime dateTime, TimeZoneInfo srcTimeZoneInfo)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, srcTimeZoneInfo);
        }

    }
}
