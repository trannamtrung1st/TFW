using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TFW.Framework.EFCore.Options;

namespace TFW.Framework.EFCore.Providers
{
    public interface IQueryFilterProvider
    {
        IEnumerable<QueryFilter> DefaultFilters { get; }
    }
}
