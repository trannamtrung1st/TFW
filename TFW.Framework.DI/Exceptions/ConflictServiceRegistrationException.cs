using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.DI.Exceptions
{
    public class ConflictServiceRegistrationException : Exception
    {
        public ConflictServiceRegistrationException()
        {
        }

        public ConflictServiceRegistrationException(string message) : base(message)
        {
        }
    }
}
