using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TFW.Framework.DI
{
    public class ConflictServicesRegistrationException : Exception
    {
        public ConflictServicesRegistrationException()
        {
        }

        public ConflictServicesRegistrationException(string message) : base(message)
        {
        }
    }
}
