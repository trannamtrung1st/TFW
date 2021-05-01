using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Docs.AppAdmin
{
    public static class SectionNames
    {
        public const string Scripts = nameof(Scripts);
        public const string Styles = nameof(Styles);
    }

    public class AppResources
    {
        private AppResources() { }

        public const string Title = nameof(Title);
        public const string Description = nameof(Description);

        public const string DefaultConfirmBtnText = nameof(DefaultConfirmBtnText);
        public const string DefaultConfirmTitle = nameof(DefaultConfirmTitle);
        public const string DefaultSuccessTitle = nameof(DefaultSuccessTitle);
        public const string DefaultErrorTitle = nameof(DefaultErrorTitle);
        public const string DefaultOkBtnText = nameof(DefaultOkBtnText);
        public const string DefaultErrorHtml = nameof(DefaultErrorHtml);
    }

    public static class Parameters
    {
        public const string ReturnUrlParam = "returnUrl";
        public const string StatusParam = "code";
    }

    public static class AppPages
    {
        public static class Folders
        {
            public const string Root = "/";
        }

        public static class Pages
        {
            public const string Login = "/Login/Index";
        }
    }
}
