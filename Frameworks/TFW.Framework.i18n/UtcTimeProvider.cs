using System;

namespace TFW.Framework.i18n
{
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
