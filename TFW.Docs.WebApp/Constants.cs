﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Docs.WebApp
{
    public static class SectionNames
    {
        public const string Scripts = nameof(Scripts);
        public const string Styles = nameof(Styles);
    }

    public static class ResourceKeys
    {
        public const string Title = nameof(Title);
        public const string Description = nameof(Description);
    }

    public static class PageConsts
    {
        public static class Admin
        {
            public const string AreaName = nameof(Admin);

            public const string Folder_Root = "/";

            public const string Page_Login = "/Login";
        }
    }

}
