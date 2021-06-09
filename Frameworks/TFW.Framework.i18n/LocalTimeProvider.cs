using System;

namespace TFW.Framework.i18n
{
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
}
