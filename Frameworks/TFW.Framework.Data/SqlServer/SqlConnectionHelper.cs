using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFW.Framework.Data.SqlServer
{
    public static class SqlConnectionHelper
    {
        public static (int minPoolSize, int maxPoolSize) ReadPoolSize(string connStr,
            int defaultMin = 50, int defaultMax = 100)
        {
            var parts = connStr.Split(';');
            var minPoolSize = parts.FirstOrDefault(part => part.Trim().StartsWith(
                SqlConnectionConsts.Options.MinPoolSize))?.Split('=')[1];
            var maxPoolSize = parts.FirstOrDefault(part => part.Trim().StartsWith(
                SqlConnectionConsts.Options.MaxPoolSize))?.Split('=')[1];

            return (minPoolSize != null ? int.Parse(minPoolSize) : defaultMin,
                maxPoolSize != null ? int.Parse(maxPoolSize) : defaultMax);
        }
    }
}
