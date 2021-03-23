﻿using Microsoft.Extensions.Localization;
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
        public static AppResult Success(object data = null, string mess = null, IStringLocalizer localizer = null)
        {
            localizer ??= BusinessContext.Current.ResultCodeLocalizer;
            return new AppResult
            {
                Code = ResultCode.Success,
                Message = mess ?? localizer[ResultCode.Success.Display().Name],
                Data = data,
            };
        }

        public static AppResult Fail(object data = null, string mess = null, IStringLocalizer localizer = null)
        {
            localizer ??= BusinessContext.Current.ResultCodeLocalizer;
            return new AppResult
            {
                Code = ResultCode.Fail,
                Message = mess ?? localizer[ResultCode.Fail.Display().Name],
                Data = data,
            };
        }

        public static AppResult Error(object data = null, string mess = null, IStringLocalizer localizer = null)
        {
            localizer ??= BusinessContext.Current.ResultCodeLocalizer;
            return new AppResult
            {
                Code = ResultCode.UnknownError,
                Message = mess ?? localizer[ResultCode.UnknownError.Display().Name],
                Data = data,
            };
        }

        public static AppResult DependencyDeleteFail(object data = null, string mess = null, IStringLocalizer localizer = null)
        {
            localizer ??= BusinessContext.Current.ResultCodeLocalizer;
            return new AppResult
            {
                Code = ResultCode.DependencyDeleteFail,
                Message = mess ?? localizer[ResultCode.DependencyDeleteFail.Display().Name],
                Data = data,
            };
        }

        public static AppResult FailValidation(object data = null, string mess = null, IStringLocalizer localizer = null)
        {
            localizer ??= BusinessContext.Current.ResultCodeLocalizer;
            return new AppResult
            {
                Code = ResultCode.FailValidation,
                Message = mess ?? localizer[ResultCode.FailValidation.Display().Name],
                Data = data,
            };
        }

        public static AppResult NotFound(object data = null, string mess = null, IStringLocalizer localizer = null)
        {
            localizer ??= BusinessContext.Current.ResultCodeLocalizer;
            return new AppResult
            {
                Code = ResultCode.NotFound,
                Message = mess ?? localizer[ResultCode.NotFound.Display().Name],
                Data = data,
            };
        }

        public static AppResult Unsupported(object data = null, string mess = null, IStringLocalizer localizer = null)
        {
            localizer ??= BusinessContext.Current.ResultCodeLocalizer;
            return new AppResult
            {
                Code = ResultCode.Unsupported,
                Message = mess ?? localizer[ResultCode.Unsupported.Display().Name],
                Data = data,
            };
        }

        public static AppResult Unauthorized(object data = null, string mess = null, IStringLocalizer localizer = null)
        {
            localizer ??= BusinessContext.Current.ResultCodeLocalizer;
            return new AppResult
            {
                Code = ResultCode.Unauthorized,
                Message = mess ?? localizer[ResultCode.Unauthorized.Display().Name],
                Data = data,
            };
        }

        public static AppResult OfCode(ResultCode code, object data = null, string mess = null, IStringLocalizer localizer = null)
        {
            localizer ??= BusinessContext.Current.ResultCodeLocalizer;
            return new AppResult
            {
                Code = code,
                Message = mess ?? localizer[code.Display().Name],
                Data = data,
            };
        }

    }
}
