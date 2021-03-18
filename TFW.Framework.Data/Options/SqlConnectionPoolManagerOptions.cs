using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Data.Options
{
    public class SqlConnectionPoolManagerOptions
    {
        public const int DefaultWatchIntervalInMinutes = 1;
        public const int DefaultRetryIntervalInSeconds = 5;
        public const int MaxAllowedRetryIntervalInSeconds = 20;

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

        private int _retryIntervalInSeconds = DefaultRetryIntervalInSeconds;
        public int RetryIntervalInSeconds
        {
            get => _retryIntervalInSeconds; set
            {
                if (value <= 0 || value > MaxAllowedRetryIntervalInSeconds)
                    throw new ArgumentException(nameof(RetryIntervalInSeconds));

                _retryIntervalInSeconds = value;
            }
        }
    }
}
