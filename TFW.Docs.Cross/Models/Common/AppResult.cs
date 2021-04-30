using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Common.Extensions;

namespace TFW.Docs.Cross.Models.Common
{
    public class AppResult<T>
    {
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public ResultCode? Code { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> Extra { get; set; } = new Dictionary<string, JToken>();
    }

    public class AppResult : AppResult<object>
    {
        public AppResult() { }

        public static AppResult Success(IStringLocalizer localizer, object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Success,
                Message = mess ?? localizer[ResultCode.Success.Display().Name],
                Data = data,
            };
        }

        public static AppResult Fail(IStringLocalizer localizer, object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Fail,
                Message = mess ?? localizer[ResultCode.Fail.Display().Name],
                Data = data,
            };
        }

        public static AppResult Error(IStringLocalizer localizer, object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.UnknownError,
                Message = mess ?? localizer[ResultCode.UnknownError.Display().Name],
                Data = data,
            };
        }

        public static AppResult DependencyDeleteFail(IStringLocalizer localizer, object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.DependencyDeleteFail,
                Message = mess ?? localizer[ResultCode.DependencyDeleteFail.Display().Name],
                Data = data,
            };
        }

        public static AppResult FailValidation(IStringLocalizer localizer, object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.FailValidation,
                Message = mess ?? localizer[ResultCode.FailValidation.Display().Name],
                Data = data,
            };
        }

        public static AppResult NotFound(IStringLocalizer localizer, object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.NotFound,
                Message = mess ?? localizer[ResultCode.NotFound.Display().Name],
                Data = data,
            };
        }

        public static AppResult Unsupported(IStringLocalizer localizer, object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Unsupported,
                Message = mess ?? localizer[ResultCode.Unsupported.Display().Name],
                Data = data,
            };
        }

        public static AppResult Unauthorized(IStringLocalizer localizer, object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Unauthorized,
                Message = mess ?? localizer[ResultCode.Unauthorized.Display().Name],
                Data = data,
            };
        }

        public static AppResult OfCode(IStringLocalizer localizer, ResultCode? code, object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = code,
                Message = mess ?? (code != null ? localizer[code.Display().Name] : null),
                Data = data,
            };
        }
    }
}
