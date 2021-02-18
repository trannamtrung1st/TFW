using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using TFW.Cross.Models.Common;

namespace TFW.Cross.Models.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException()
        {
        }

        public BaseException(string message) : base(message)
        {
        }

        public BaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AppResult Result { get; protected set; }
    }
}
