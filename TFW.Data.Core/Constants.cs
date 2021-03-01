using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace TFW.Data.Core
{
    public static class DataConsts
    {
        public const string ConnStrVarName = "conn_str";
        public const string ConnStrKey = nameof(DataContext);
    }

    public static class EntityConfigConsts
    {
        public const int UserKeyStringLength = 100;

        public const int DefaultTitleLikeStringLength = 255;
        public const int DefaultCodeLikeStringLength = 100;
        public const int DefaultDescriptionLikeStringLength = 2000;

        public static readonly IEnumerable<string> CommonTitleLikeColumnEndWiths =
            ImmutableArray.Create("Name", "Title", "Subject", "Caption", "Label", "Tag");

        public static readonly IEnumerable<string> CommonCodeLikeColumnEndWiths =
            ImmutableArray.Create("Code", "Key");

        public static readonly IEnumerable<string> CommonDescriptionLikeColumnEndWiths =
            ImmutableArray.Create("Description", "Overview", "Overall");
    }

}
