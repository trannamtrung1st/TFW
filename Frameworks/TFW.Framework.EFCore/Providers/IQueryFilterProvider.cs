using System.Collections.Generic;
using TFW.Framework.EFCore.Options;

namespace TFW.Framework.EFCore.Providers
{
    public interface IQueryFilterProvider
    {
        IEnumerable<QueryFilter> DefaultFilters { get; }
    }
}
