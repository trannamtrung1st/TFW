using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Data.Options
{
    // No effect when changing after initialized
    public class SqlConnectionPoolOptions
    {
        public const int DefaultLifetimeInMinutes = 60;
        public const int DefaultMaximumRetryWhenFailure = 3;
        public const int MaximumRetryAllowed = 10;

        public string ConnectionString { get; set; }
        public int LifetimeInMinutes { get; set; } = DefaultLifetimeInMinutes;
        public int MaximumConnections { get; set; }
        public int MinimumConnections { get; set; }

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

        internal SqlConnectionPoolOptions DeepClone()
        {
            return new SqlConnectionPoolOptions
            {
                ConnectionString = ConnectionString,
                LifetimeInMinutes = LifetimeInMinutes,
                MaximumConnections = MaximumConnections,
                MaximumRetryWhenFailure = MaximumRetryWhenFailure,
                MinimumConnections = MinimumConnections
            };
        }
    }
}
