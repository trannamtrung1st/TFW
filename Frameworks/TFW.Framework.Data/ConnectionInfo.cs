using System;
using TFW.Framework.Data.Wrappers;

namespace TFW.Framework.Data
{
    internal struct ConnectionInfo
    {
        public PooledDbConnectionWrapper Wrapper { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
