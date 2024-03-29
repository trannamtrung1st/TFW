﻿using System.Data;
using System.Data.Common;

namespace TFW.Framework.Data.Helpers
{
    public static class DbConnectionExtensions
    {
        public static bool CanOpen(this DbConnection connection)
        {
            return !string.IsNullOrWhiteSpace(connection.ConnectionString);
        }

        public static bool IsOpening(this DbConnection connection)
        {
            var connectionState = connection.State;

            return (connectionState != ConnectionState.Closed && connectionState != ConnectionState.Broken);
        }
    }
}
