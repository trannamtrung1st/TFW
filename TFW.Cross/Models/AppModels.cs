using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using TFW.Framework.Common;

namespace TFW.Cross.Models
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
                Message = mess ?? ResultCode.Success.Description(),
                Data = data,
            };
        }

        public static AppResult Fail(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Fail,
                Message = mess ?? ResultCode.Fail.Description(),
                Data = data,
            };
        }

        public static AppResult Error(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.UnknownError,
                Message = mess ?? ResultCode.UnknownError.Description(),
                Data = data,
            };
        }

        public static AppResult DependencyDeleteFail(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.DependencyDeleteFail,
                Message = mess ?? ResultCode.DependencyDeleteFail.Description(),
                Data = data,
            };
        }

        public static AppResult FailValidation(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.FailValidation,
                Message = mess ?? ResultCode.FailValidation.Description(),
                Data = data,
            };
        }

        public static AppResult NotFound(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.NotFound,
                Message = mess ?? ResultCode.NotFound.Description(),
                Data = data,
            };
        }

        public static AppResult Unsupported(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Unsupported,
                Message = mess ?? ResultCode.Unsupported.Description(),
                Data = data,
            };
        }

        public static AppResult Unauthorized(object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = ResultCode.Unauthorized,
                Message = mess ?? ResultCode.Unauthorized.Description(),
                Data = data,
            };
        }

        public static AppResult OfCode(ResultCode code, object data = null, string mess = null)
        {
            return new AppResult
            {
                Code = code,
                Message = mess ?? code.Description(),
                Data = data,
            };
        }

    }

    public class ValidationData
    {
        [JsonProperty("isValid")]
        public bool IsValid { get; set; }
        [JsonProperty("details")]
        public List<AppResult> Details { get; set; }
        [JsonIgnore]
        public IDictionary<string, object> TempData { get; set; }

        public ValidationData()
        {
            Details = new List<AppResult>();
            IsValid = true;
            TempData = new Dictionary<string, object>();
        }

        public T GetTempData<T>(string key)
        {
            object data = null;
            if (TempData.TryGetValue(key, out data))
                return (T)data;
            return default;
        }

        public ValidationData Fail(string mess = null, ResultCode? code = null, object data = null)
        {
            Details.Add(new AppResult
            {
                Message = mess ?? code?.Description(),
                Data = data,
                Code = code
            });
            IsValid = false;
            return this;
        }

    }

    public class GetListResponseModel<T>
    {
        [JsonProperty("list")]
        public T[] List { get; set; }
        [JsonProperty("totalCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalCount { get; set; }
    }

    public class PrincipalInfo
    {
        public string UserId { get; set; }
    }
}
