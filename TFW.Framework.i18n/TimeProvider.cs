using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.i18n.Helpers;

namespace TFW.Framework.i18n
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
        DateTime Today { get; }
        DateTimeOffset OffsetNow { get; }
        DateTimeKind Kind { get; }
        DateTime Normalize(DateTime dateTime);
    }

    public interface IZoneSpecificTimeProvider : ITimeProvider
    {
        TimeZoneInfo Current { get; set; }
        void SetCurrentByTimeZoneId(string timeZoneId);
    }

    public class DefaultZoneSpecificTimeProvider : IZoneSpecificTimeProvider
    {
        public DateTime Now => DateTime.UtcNow.ToTimeZoneFromUtc(_current);

        public DateTime Today => Now.Date;

        public DateTimeOffset OffsetNow => DateTimeOffset.UtcNow.ToTimeZone(_current);

        public DateTimeKind Kind => _current.Id == TimeZoneInfo.Utc.Id ? DateTimeKind.Utc :
            (_current.Id == TimeZoneInfo.Local.Id ? DateTimeKind.Local : DateTimeKind.Unspecified);

        public DateTime Normalize(DateTime dateTime)
        {
            return dateTime.ToTimeZone(_current);
        }

        public void SetCurrentByTimeZoneId(string timeZoneId)
        {
            Current = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }

        private TimeZoneInfo _current = TimeZoneInfo.Local;
        public TimeZoneInfo Current
        {
            get => _current;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _current = value;
            }
        }
    }

    public class FixedZoneTimeProvider : ITimeProvider
    {
        private readonly TimeZoneInfo _timeZoneInfo;

        public FixedZoneTimeProvider(string timeZoneId)
        {
            if (timeZoneId == null)
                throw new ArgumentNullException(nameof(timeZoneId));

            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }

        public FixedZoneTimeProvider(TimeZoneInfo timeZoneInfo)
        {
            if (timeZoneInfo == null)
                throw new ArgumentNullException(nameof(timeZoneInfo));

            _timeZoneInfo = timeZoneInfo;
        }

        public DateTime Now => DateTime.UtcNow.ToTimeZoneFromUtc(_timeZoneInfo);

        public DateTime Today => Now.Date;

        public DateTimeOffset OffsetNow => DateTimeOffset.UtcNow.ToTimeZone(_timeZoneInfo);

        public DateTimeKind Kind => _timeZoneInfo.Id == TimeZoneInfo.Utc.Id ? DateTimeKind.Utc :
            (_timeZoneInfo.Id == TimeZoneInfo.Local.Id ? DateTimeKind.Local : DateTimeKind.Unspecified);

        public DateTime Normalize(DateTime dateTime)
        {
            return dateTime.ToTimeZone(_timeZoneInfo);
        }
    }

    public class LocalTimeProvider : ITimeProvider
    {
        internal LocalTimeProvider() { }

        public DateTime Now => DateTime.Now;

        public DateTimeKind Kind => DateTimeKind.Local;

        public DateTimeOffset OffsetNow => DateTimeOffset.Now;

        public DateTime Today => Now.Date;

        public DateTime Normalize(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);

            if (dateTime.Kind == DateTimeKind.Utc)
                return dateTime.ToLocalTime();

            return dateTime;
        }
    }

    public class UnspecifiedTimeProvider : ITimeProvider
    {
        internal UnspecifiedTimeProvider() { }

        public DateTime Now => DateTime.Now;

        public DateTimeKind Kind => DateTimeKind.Unspecified;

        public DateTimeOffset OffsetNow => DateTimeOffset.Now;

        public DateTime Today => Now.Date;

        public DateTime Normalize(DateTime dateTime)
        {
            return dateTime;
        }

    }

    public class UtcTimeProvider : ITimeProvider
    {
        internal UtcTimeProvider() { }

        public DateTime Now => DateTime.UtcNow;

        public DateTimeKind Kind => DateTimeKind.Utc;

        public DateTimeOffset OffsetNow => DateTimeOffset.UtcNow;

        public DateTime Today => Now.Date;

        public DateTime Normalize(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            if (dateTime.Kind == DateTimeKind.Local)
                return dateTime.ToUniversalTime();

            return dateTime;
        }
    }
}
