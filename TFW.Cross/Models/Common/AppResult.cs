using Newtonsoft.Json;
using System.Text;
using TFW.Framework.Common;

namespace TFW.Cross.Models.Common
{
    public class AppResult
    {
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public ResultCode? Code { get; set; }

        public static AppResult Success(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Success,
                Message = mess ?? ResultCode.Success.DisplayName(),
                Data = data,
            };
        }

        public static AppResult Fail(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Fail,
                Message = mess ?? ResultCode.Fail.DisplayName(),
                Data = data,
            };
        }

        public static AppResult Error(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.UnknownError,
                Message = mess ?? ResultCode.UnknownError.DisplayName(),
                Data = data,
            };
        }

        public static AppResult DependencyDeleteFail(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.DependencyDeleteFail,
                Message = mess ?? ResultCode.DependencyDeleteFail.DisplayName(),
                Data = data,
            };
        }

        public static AppResult FailValidation(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.FailValidation,
                Message = mess ?? ResultCode.FailValidation.DisplayName(),
                Data = data,
            };
        }

        public static AppResult NotFound(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.NotFound,
                Message = mess ?? ResultCode.NotFound.DisplayName(),
                Data = data,
            };
        }

        public static AppResult Unsupported(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Unsupported,
                Message = mess ?? ResultCode.Unsupported.DisplayName(),
                Data = data,
            };
        }

        public static AppResult Unauthorized(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Unauthorized,
                Message = mess ?? ResultCode.Unauthorized.DisplayName(),
                Data = data,
            };
        }

        public static AppResult OfCode(ResultCode code, object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = code,
                Message = mess ?? code.DisplayName(),
                Data = data,
            };
        }

    }
}
