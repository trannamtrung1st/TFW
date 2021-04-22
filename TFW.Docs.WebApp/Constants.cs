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

    public static class ConfigConsts
    {
        public static class i18n
        {
            public const string ResourcesPath = "Resources";
        }
    }

    public static class Routing
    {
        public static class Page
        {
            public const string Index = "/";

            public static class Post
            {
                public const string Index = "/post";
            }
        }

        public static class Api
        {
        }
    }
}