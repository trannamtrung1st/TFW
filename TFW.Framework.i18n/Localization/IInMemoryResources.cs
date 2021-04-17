using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.i18n.Localization
{
    public interface IInMemoryResources
    {
        public Type SourceType { get; }
        public IDictionary<string, IDictionary<string, string>> Resources { get; }
    }
}
