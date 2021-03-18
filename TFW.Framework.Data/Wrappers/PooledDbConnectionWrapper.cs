using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace TFW.Framework.Data.Wrappers
{
    internal class PooledDbConnectionWrapper<TConnection> where TConnection : DbConnection
    {
        private readonly TConnection _conn;
        private readonly string _poolKey;

        public PooledDbConnectionWrapper(TConnection conn, string poolKey)
        {
            _conn = conn;
            _poolKey = poolKey;
        }

        public TConnection DbConnection => _conn;
        public string PoolKey => _poolKey;
    }

    internal class SqlPooledDbConnectionWrapper : PooledDbConnectionWrapper<SqlConnection>
    {
        public SqlPooledDbConnectionWrapper(SqlConnection conn, string poolKey) : base(conn, poolKey)
        {
        }
    }
}
