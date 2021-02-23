using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Providers
{
    public interface IBusinessContextProvider
    {
        public BusinessContext BusinessContext { get; }
    }
}
