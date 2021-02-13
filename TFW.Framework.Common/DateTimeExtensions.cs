using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TFW.Framework.Common
{

    public static class DateTimeExtensions
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

        public static DateTime ToTimeZone(this DateTime utcDate, string timeZoneId)
        {
            TimeZoneInfo sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, sourceTimeZone);
            return localDate;
        }

        public static DateTime ToTimeZone(this DateTime utcDate, TimeZoneInfo timeZoneInfo)
        {
            DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, timeZoneInfo);
            return localDate;
        }

        public static DateTime ToUtc(this DateTime localDate, TimeZoneInfo srcTimeZoneInfo)
        {
            return TimeZoneInfo.ConvertTimeToUtc(localDate, srcTimeZoneInfo);
        }

    }
}
