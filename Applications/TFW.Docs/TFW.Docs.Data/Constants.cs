using System.Collections.Generic;

namespace TFW.Docs.Data
{
    public static class DataConsts
    {
        public const string ConnStrKey = "ConnectionStrings:DataContext";
    }

    public static class EntityConfigConsts
    {
        public const int DefaultTitleLikeStringLength = 256;
        public const int DefaultCodeLikeStringLength = 100;
        public const int DefaultDescriptionLikeStringLength = 2000;

        public static readonly IEnumerable<string> CommonTitleLikeColumnEndWiths
            = new[] { "Name", "Title", "Subject", "Caption", "Label", "Tag" };

        public static readonly IEnumerable<string> CommonCodeLikeColumnEndWiths = new[] { "Code", "Key" };

        public static readonly IEnumerable<string> CommonDescriptionLikeColumnEndWiths
            = new[] { "Description", "Overview", "Overall" };
    }
}
