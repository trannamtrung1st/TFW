﻿namespace TFW.Framework.Data
{
    public static class SqlServerColumnType
    {
        public const string ntext = nameof(ntext);
        public const string text = nameof(text);
    }

    public static class SqlConnectionConsts
    {
        public static class Options
        {
            public const string MinPoolSize = "Min Pool Size";
            public const string MaxPoolSize = "Min Pool Size";
            public const string Password = "Password";
        }
    }
}
