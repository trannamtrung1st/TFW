using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using TFW.Framework.Data;

namespace TFW.Framework.EFCore
{
    public static class QueryFilterConsts
    {
        public const string SoftDeleteDefaultName = "SoftDelete";
    }

    public static class SqlServerConsts
    {
        public static readonly IEnumerable<string> TextColumnTypes = ImmutableArray.Create(
            SqlServerColumnType.text, SqlServerColumnType.ntext);
    }
}
