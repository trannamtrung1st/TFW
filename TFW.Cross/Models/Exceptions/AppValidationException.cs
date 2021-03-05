﻿using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Models.Common;

namespace TFW.Cross.Models.Exceptions
{
    public class AppValidationException : BaseException
    {
        private AppValidationException(ValidationData validationData) : base(validationData?.Message)
        {
            if (validationData == null)
                throw new ArgumentNullException(nameof(validationData));

            Result = AppResult.FailValidation(validationData, mess: validationData.Message);
        }

        public static AppValidationException From(ValidationData validationData)
        {
            return new AppValidationException(validationData);
        }

        public static AppValidationException From(ResultCode resultCode, object data = null, string mess = null)
        {
            var validationData = new ValidationData()
                .Fail(mess, resultCode, data);

            return From(validationData);
        }
    }
}
