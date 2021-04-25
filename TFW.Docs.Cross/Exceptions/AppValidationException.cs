using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Models.Common;

namespace TFW.Docs.Cross.Exceptions
{
    public class AppValidationException : BaseException
    {
        private AppValidationException(IStringLocalizer localizer, ValidationData validationData = null)
            : base(validationData?.Message)
        {
            Result = AppResult.FailValidation(localizer, validationData, mess: validationData?.Message);
        }

        public static AppValidationException New(IStringLocalizer localizer)
        {
            return new AppValidationException(localizer);
        }

        public static AppValidationException From(IStringLocalizer localizer, ValidationData validationData)
        {
            return new AppValidationException(localizer, validationData);
        }

        public static AppValidationException From(IStringLocalizer localizer, ResultCode resultCode, object data = null, string mess = null)
        {
            var validationData = new ValidationData(localizer)
                .Fail(mess, resultCode, data);

            return From(localizer, validationData);
        }
    }
}
