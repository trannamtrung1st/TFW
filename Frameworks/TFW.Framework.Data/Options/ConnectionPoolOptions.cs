using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Data.Options
{
    // No effect when changing after initialized
    public class ConnectionPoolOptions
    {
        public const int DefaultLifetimeInMinutes = 60;
        public const int DefaultMaximumRetryWhenFailure = 3;
        public const int MaximumRetryAllowed = 10;
        public const int MaxAllowedRetryIntervalInSeconds = 20;
        public const int DefaultRetryIntervalInSeconds = 5;

        public string ConnectionString { get; set; }
        public int LifetimeInMinutes { get; set; } = DefaultLifetimeInMinutes;
        public int MaxPoolSize { get; set; }
        public int MinPoolSize { get; set; }

        private int _maximumRetryWhenFailure = DefaultMaximumRetryWhenFailure;
        public int MaximumRetryWhenFailure
        {
            get => _maximumRetryWhenFailure; set
            {
                if (value < 0)
                    throw new ArgumentException(nameof(MaximumRetryWhenFailure));

                if (value > MaximumRetryAllowed)
                    throw new ArgumentException($"Maximum : {MaximumRetryAllowed}");

                _maximumRetryWhenFailure = value;
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

        internal ConnectionPoolOptions Snapshot()
        {
            return new ConnectionPoolOptions
            {
                ConnectionString = ConnectionString,
                LifetimeInMinutes = LifetimeInMinutes,
                MaxPoolSize = MaxPoolSize,
                MaximumRetryWhenFailure = MaximumRetryWhenFailure,
                MinPoolSize = MinPoolSize
            };
        }
    }
}
