using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using TFW.Docs.Cross;
using TFW.Docs.AppAdmin.Pages.Shared;
using System.Net;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace TFW.Docs.AppAdmin.Pages
{
    using Resources = AppResources.Pages.StatusCodeModel;

    public class StatusCodeModel : BasePageModel<StatusCodeModel>, IPageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly IStringLocalizer<ResultCodeResources> _resultLocalizer;

        public StatusCodeModel(
            IStringLocalizer<StatusCodeModel> localizer,
            IStringLocalizer<ResultCodeResources> resultLocalizer,
            IWebHostEnvironment env) : base(localizer)
        {
            _resultLocalizer = resultLocalizer;
            _env = env;
        }

        public string Message { get; set; }
        public int Code { get; set; }
        public string Layout { get; set; } = null;
        public string MessageTitle { get; set; }
        public string StatusCodeStyle { get; set; }
        public string OriginalUrl { get; set; }
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public IActionResult OnGet(int? code = null)
        {
            return OnAll(code);
        }

        public IActionResult OnPost(int? code = null)
        {
            return OnAll(code);
        }

        private IActionResult OnAll(int? code = null)
        {
            var statusCodeReExecuteFeature =
                HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error != null)
                return HandleError(exceptionHandlerPathFeature);

            if (statusCodeReExecuteFeature != null)
                return HandleStatus(code.Value, statusCodeReExecuteFeature);

            return LocalRedirect(Routing.App.Index);
        }

        private IActionResult HandleError(IExceptionHandlerPathFeature feature)
        {
            var ex = feature.Error;
            AppResult result;

            if (ex is AppValidationException validEx)
            {
                result = validEx.Result;
                StatusCodeStyle = Resources.BadRequestMessageStyle;
                MessageTitle = Localizer[Resources.BadRequestMessageTitle];
                Code = (int)HttpStatusCode.BadRequest;
            }
            else if (ex is AuthorizationException authEx)
            {
                if (authEx.IsUnauthorized)
                {
                    result = AppResult.Unauthorized(_resultLocalizer);
                    StatusCodeStyle = Resources.UnauthorizedMessageStyle;
                    MessageTitle = Localizer[Resources.UnauthorizedMessageTitle];
                    Code = (int)HttpStatusCode.Unauthorized;
                }
                else
                {
                    result = AppResult.AccessDenied(_resultLocalizer);
                    StatusCodeStyle = Resources.AccessDeniedMessageStyle;
                    MessageTitle = Localizer[Resources.AccessDeniedMessageTitle];
                    Code = (int)HttpStatusCode.Forbidden;
                }
            }
            else if (ex is AppException appEx)
            {
                result = appEx.Result;
                StatusCodeStyle = Resources.ErrorMessageStyle;
                MessageTitle = Localizer[Resources.ErrorMessageTitle];
                Code = (int)HttpStatusCode.InternalServerError;
            }
            else
            {
                if (_env.IsDevelopment())
                    result = AppResult.Error(_resultLocalizer, data: ex, mess: ex.Message);
                else result = AppResult.Error(_resultLocalizer, Localizer[Resources.ErrorMessage]);

                StatusCodeStyle = Resources.ErrorMessageStyle;
                MessageTitle = Localizer[Resources.ErrorMessageTitle];
                Code = (int)HttpStatusCode.InternalServerError;
            }

            Message = result.Message;
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            OriginalUrl = feature.Path;

            return Page();
        }

        private IActionResult HandleStatus(int code, IStatusCodeReExecuteFeature feature)
        {
            OriginalUrl =
                feature.OriginalPathBase
                + feature.OriginalPath
                + feature.OriginalQueryString;
            Code = code;
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            switch (Code)
            {
                case (int)HttpStatusCode.NotFound:
                    StatusCodeStyle = Resources.NotFoundMessageStyle;
                    MessageTitle = Localizer[Resources.NotFoundMessageTitle];
                    Message = Localizer[Resources.NotFoundMessage];
                    break;
                case (int)HttpStatusCode.Forbidden:
                    StatusCodeStyle = Resources.AccessDeniedMessageStyle;
                    MessageTitle = Localizer[Resources.AccessDeniedMessageTitle];
                    Message = Localizer[Resources.AccessDeniedMessage];
                    break;
                case (int)HttpStatusCode.Unauthorized:
                    StatusCodeStyle = Resources.UnauthorizedMessageStyle;
                    MessageTitle = Localizer[Resources.UnauthorizedMessageTitle];
                    Message = Localizer[Resources.UnauthorizedMessage];
                    break;
                case (int)HttpStatusCode.BadRequest:
                    StatusCodeStyle = Resources.BadRequestMessageStyle;
                    MessageTitle = Localizer[Resources.BadRequestMessageTitle];
                    Message = Localizer[Resources.BadRequestMessage];
                    break;
                default:
                    StatusCodeStyle = Resources.CommonMessageStyle;
                    MessageTitle = Localizer[Resources.CommonMessageTitle];
                    Message = Localizer[Resources.CommonMessage];
                    break;
            }

            return Page();
        }
    }
}
