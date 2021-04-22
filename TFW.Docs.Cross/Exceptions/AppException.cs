using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Models.Common;

namespace TFW.Docs.Cross.Exceptions
{
    public class AppException : BaseException
    {
        private AppException(AppResult result) : base(result?.Message)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            Result = result;
        }

        public static AppException From(AppResult appResult)
        {
            return new AppException(result: appResult);
        }

        public static AppException From(IStringLocalizer localizer, ResultCode code, object data = null, string mess = null)
        {
            return From(AppResult.OfCode(localizer, code, data, mess));
        }
    }
}
