using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Common.Helpers;

namespace TFW.Cross.Models.Common
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
        public IDictionary<string, object> Extra { get; set; } = new Dictionary<string, object>();
    }

    public class AppResult : AppResult<object>
    {
        public static AppResult Success(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Success,
                Message = mess ?? ResultCode.Success.Display().Name,
                Data = data,
            };
        }

        public static AppResult Fail(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Fail,
                Message = mess ?? ResultCode.Fail.Display().Name,
                Data = data,
            };
        }

        public static AppResult Error(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.UnknownError,
                Message = mess ?? ResultCode.UnknownError.Display().Name,
                Data = data,
            };
        }

        public static AppResult DependencyDeleteFail(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.DependencyDeleteFail,
                Message = mess ?? ResultCode.DependencyDeleteFail.Display().Name,
                Data = data,
            };
        }

        public static AppResult FailValidation(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.FailValidation,
                Message = mess ?? ResultCode.FailValidation.Display().Name,
                Data = data,
            };
        }

        public static AppResult NotFound(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.NotFound,
                Message = mess ?? ResultCode.NotFound.Display().Name,
                Data = data,
            };
        }

        public static AppResult Unsupported(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Unsupported,
                Message = mess ?? ResultCode.Unsupported.Display().Name,
                Data = data,
            };
        }

        public static AppResult Unauthorized(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Unauthorized,
                Message = mess ?? ResultCode.Unauthorized.Display().Name,
                Data = data,
            };
        }

        public static AppResult OfCode(ResultCode code, object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = code,
                Message = mess ?? code.Display().Name,
                Data = data,
            };
        }

    }
}
