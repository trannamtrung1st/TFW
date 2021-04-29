using System;
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

    public static class WebAppConsts
    {
        public static class Admin
        {
            public const string AreaName = nameof(Admin);

            public const string Folder_Root = "/";

            public const string Page_Login = "/Login/Index";

            public const string ReturnUrlParameter = "returnUrl";
        }
    }

}
