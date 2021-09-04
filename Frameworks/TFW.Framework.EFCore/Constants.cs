using System.Collections.Generic;
using TFW.Framework.Data;

namespace TFW.Framework.EFCore
{
    public static class QueryFilterConsts
    {
        public const string SoftDeleteDefaultName = "SoftDelete";
    }

    public static class SqlServerConsts
    {
        public static readonly IEnumerable<string> TextColumnTypes = new[]
        {
            SqlServerColumnType.text,
            SqlServerColumnType.ntext
        };
    }
}
