using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.Cross.Providers
{
    public interface IBusinessContextProvider
    {
        BusinessContext BusinessContext { get; }
    }
}
