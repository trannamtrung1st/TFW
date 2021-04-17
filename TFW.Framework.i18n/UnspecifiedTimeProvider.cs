using System;

namespace TFW.Framework.i18n
{
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
}
