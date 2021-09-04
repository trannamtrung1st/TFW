using System;

namespace TFW.Framework.Data.Options
{
    public class SqlConnectionPoolManagerOptions
    {
        public const int DefaultWatchIntervalInMinutes = 1;

        private int _watchIntervalInMinutes = DefaultWatchIntervalInMinutes;
        public int WatchIntervalInMinutes
        {
            get => _watchIntervalInMinutes; set
            {
                if (value <= 0)
                    throw new ArgumentException(nameof(WatchIntervalInMinutes));

                _watchIntervalInMinutes = value;
            }
        }
    }
}
