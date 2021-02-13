using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Common;

namespace TFW.Cross.Models.Exceptions
{
    public class AppException : Exception
    {
        public AppError Error { get; }

        private AppException(AppError error) : this(error, message: error.Description())
        {
        }

        private AppException(AppError error, string message) : base(message)
        {
            Error = error;
        }

        public static AppException Create(AppError error)
        {
            return new AppException(error);
        }

        public static AppException Create(AppError error, string message)
        {
            return new AppException(error, message);
        }
    }
}
