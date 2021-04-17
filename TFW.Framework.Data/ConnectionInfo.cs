using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Data.Wrappers;

namespace TFW.Framework.Data
{
    internal struct ConnectionInfo
    {
        public PooledDbConnectionWrapper Wrapper { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
