using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.EFCore
{
    public static class QueryFilterConsts
    {
        public const string SoftDeleteDefaultName = "SoftDelete";
    }

    public static class SqlServerConsts
    {
        public static readonly string[] TextColumnTypes = new[] { "text", "ntext" };
    }
}
