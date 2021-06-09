using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace TFW.Framework.Data.Wrappers
{
    internal class PooledDbConnectionWrapper
    {
        protected readonly DbConnection _conn;
        protected readonly string _poolKey;

        public PooledDbConnectionWrapper(DbConnection conn, string poolKey)
        {
            _conn = conn;
            _poolKey = poolKey;
        }

        public DbConnection DbConnection => _conn;
        public string PoolKey => _poolKey;
        public bool IsInPool { get; set; } = false;
    }
}
