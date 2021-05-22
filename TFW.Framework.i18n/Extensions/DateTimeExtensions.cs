using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TFW.Framework.i18n.Extensions
{
    public static class DateTimeExtensions
    {
        public static int ToAge(this DateTime utcBirthday)
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - utcBirthday;
            return (int)timeSpan.TotalDays / 365;
        }

        public static DateTime LastMonthEnd(this DateTime dateTime)
        {
            return dateTime.GetMonthStart().AddSeconds(-1);
        }

        public static DateTime LastMonthStart(this DateTime dateTime)
        {
            return dateTime.GetMonthStart().AddMonths(-1);
        }

        public static DateTime GetMonthEnd(this DateTime dateTime)
        {
            return dateTime.GetMonthStart().AddMonths(1).AddSeconds(-1);
        }

        public static DateTime GetMonthStart(this DateTime dateTime)
        {
            return dateTime.AddDays(1 - dateTime.Day).Date;
        }

        public static DateTime LastWeekEnd(this DateTime dateTime)
        {
            return dateTime.GetWeekStart().AddSeconds(-1);
        }

        public static DateTime LastWeekStart(this DateTime dateTime)
        {
            return dateTime.GetWeekStart().AddDays(-7);
        }

        public static DateTime GetWeekEnd(this DateTime dateTime)
        {
            return dateTime.GetWeekStart().AddDays(7).AddSeconds(-1);
        }

        public static DateTime GetWeekStart(this DateTime dateTime)
        {
            return dateTime.AddDays(-(int)dateTime.DayOfWeek).Date;
        }

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

        #region TimeZone
        public static DateTime ToTimeZone(this DateTime dateTime, TimeZoneInfo destTimeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTime, destTimeZone);
        }

        public static DateTime ToTimeZone(this DateTime dateTime, string destTimeZoneId)
        {
            TimeZoneInfo destTimeZone = TimeZoneInfo.FindSystemTimeZoneById(destTimeZoneId);
            return TimeZoneInfo.ConvertTime(dateTime, destTimeZone);
        }

        public static DateTime ToTimeZone(this DateTime dateTime, TimeZoneInfo srcTimeZone, TimeZoneInfo destTimeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTime, srcTimeZone, destTimeZone);
        }

        public static DateTime ToTimeZone(this DateTime dateTime, string srcTimeZoneId, string destTimeZoneId)
        {
            TimeZoneInfo destTimeZone = TimeZoneInfo.FindSystemTimeZoneById(destTimeZoneId);
            TimeZoneInfo srcTimeZone = TimeZoneInfo.FindSystemTimeZoneById(srcTimeZoneId);

            return TimeZoneInfo.ConvertTime(dateTime, srcTimeZone, destTimeZone);
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
        #endregion

        public static DateTime Adjust(this DateTime dateTime, DateTimeKind kind)
        {
            return DateTime.SpecifyKind(dateTime, kind);
        }
    }
}
