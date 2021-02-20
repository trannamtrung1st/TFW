using System;
using System.Collections.Generic;
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

        public static readonly string[] CommonTitleLikeColumnEndWiths =
            new[] { "Name", "Title", "Subject", "Caption", "Label", "Tag" };

        public static readonly string[] CommonCodeLikeColumnEndWiths =
            new[] { "Code", "Key" };

        public static readonly string[] CommonDescriptionLikeColumnEndWiths =
            new[] { "Description", "Overview", "Overall" };
    }

}
