using System;

namespace TFW.Framework.DI.Exceptions
{
    public class ServiceRegistrationException : Exception
    {
        public ServiceRegistrationException()
        {
        }

        public ServiceRegistrationException(string message) : base(message)
        {
        }
    }
}
