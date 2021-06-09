using System;
using TFW.Framework.i18n.Extensions;

namespace TFW.Framework.i18n
{
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
}
