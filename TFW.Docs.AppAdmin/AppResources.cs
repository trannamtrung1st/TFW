using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Docs.AppAdmin
{
    public class AppResources
    {
        public const string Title = nameof(Title);
        public const string Description = nameof(Description);

        public const string DefaultConfirmBtnText = nameof(DefaultConfirmBtnText);
        public const string DefaultConfirmTitle = nameof(DefaultConfirmTitle);
        public const string DefaultSuccessTitle = nameof(DefaultSuccessTitle);
        public const string DefaultErrorTitle = nameof(DefaultErrorTitle);
        public const string DefaultOkBtnText = nameof(DefaultOkBtnText);
        public const string DefaultErrorHtml = nameof(DefaultErrorHtml);

        public static class Pages
        {
            public static class Login
            {
                public static class IndexModel
                {
                    public const string InitSuccess = nameof(InitSuccess);
                    public const string ConfirmInit = nameof(ConfirmInit);
                    public const string InvalidUsernameOrPassword = nameof(InvalidUsernameOrPassword);
                }
            }

            public static class StatusCodeModel
            {
                public const string ErrorMessage = nameof(ErrorMessage);
                public const string ErrorMessageTitle = nameof(ErrorMessageTitle);
                public const string ErrorMessageStyle = "danger";

                public const string BadRequestMessage = nameof(BadRequestMessage);
                public const string BadRequestMessageTitle = nameof(BadRequestMessageTitle);
                public const string BadRequestMessageStyle = "warning";

                public const string UnauthorizedMessage = nameof(UnauthorizedMessage);
                public const string UnauthorizedMessageTitle = nameof(UnauthorizedMessageTitle);
                public const string UnauthorizedMessageStyle = "danger";

                public const string AccessDeniedMessage = nameof(AccessDeniedMessage);
                public const string AccessDeniedMessageTitle = nameof(AccessDeniedMessageTitle);
                public const string AccessDeniedMessageStyle = "danger";

                public const string NotFoundMessage = nameof(NotFoundMessage);
                public const string NotFoundMessageTitle = nameof(NotFoundMessageTitle);
                public const string NotFoundMessageStyle = "secondary";

                public const string CommonMessage = nameof(CommonMessage);
                public const string CommonMessageTitle = nameof(CommonMessageTitle);
                public const string CommonMessageStyle = "warning";
            }
        }
    }
}
