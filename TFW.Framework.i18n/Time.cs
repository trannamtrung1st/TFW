using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.i18n
{
    public static class Time
    {
        public static DateTime Now => Providers.Default.Now;
        public static DateTime Today => Providers.Default.Today;
        public static DateTimeOffset OffsetNow => Providers.Default.OffsetNow;
        public static DateTimeKind Kind => Providers.Default.Kind;
        public static DateTime Normalize(DateTime dateTime) => Providers.Default.Normalize(dateTime);

        //[ThreadStatic] // ThreadStatic does not maintain value between Tasks
        //private static TimeZoneInfo _threadTimeZone = TimeZoneInfo.Local;
        //public static TimeZoneInfo ThreadTimeZone
        //{
        //    get => _threadTimeZone;
        //    set
        //    {
        //        if (value == null)
        //            throw new ArgumentNullException(nameof(value));

        //        _threadTimeZone = value;
        //    }
        //}

        public static IThreadTimeZoneProvider ThreadTimeZoneProvider { get; set; }
        public static TimeZoneInfo ThreadTimeZone
        {
            get => ThreadTimeZoneProvider?.TimeZone;
            set
            {
                if (ThreadTimeZoneProvider != null)
                    ThreadTimeZoneProvider.TimeZone = value;
            }
        }

        public static class Providers
        {
            private static ITimeProvider _default;
            public static ITimeProvider Default
            {
                get { return _default; }
                set
                {
                    if (value == null)
                        throw new ArgumentNullException(nameof(value));

                    _default = value;
                }
            }
            public static ITimeProvider Utc { get; set; }
            public static ITimeProvider Local { get; set; }
            public static ITimeProvider Unspecified { get; set; }
            public static ITimeProvider FixedTimeZone { get; set; }
            public static ITimeProvider ZoneSpecific { get; set; }

            static Providers()
            {
                Utc = new UtcTimeProvider();
                Local = new LocalTimeProvider();
                Unspecified = new UnspecifiedTimeProvider();
                FixedTimeZone = new FixedZoneTimeProvider(TimeZoneInfo.Local);
                ZoneSpecific = new DefaultZoneSpecificTimeProvider();
                _default = Utc;
            }
        }
    }
}
