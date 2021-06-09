using System;
using TFW.Framework.i18n.Extensions;

namespace TFW.Framework.i18n
{
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
}
