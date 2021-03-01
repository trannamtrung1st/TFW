using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace TFW.Framework.EFCore
{
    public static class QueryFilterConsts
    {
        public const string SoftDeleteDefaultName = "SoftDelete";
    }

    public static class SqlServerConsts
    {
        public static readonly IEnumerable<string> TextColumnTypes = ImmutableArray.Create("text", "ntext");
    }
}
