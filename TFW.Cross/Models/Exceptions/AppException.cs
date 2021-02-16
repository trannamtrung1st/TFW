using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Models.Exceptions
{
    public class AppException : Exception
    {
        public AppResult Result { get; }

        private AppException(AppResult result) : base(result?.Message)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            Result = result;
        }

        public static AppException From(ResultCode code, object data = null, string mess = null)
        {
            return new AppException(result: AppResult.OfCode(code, data, mess));
        }
    }
}
