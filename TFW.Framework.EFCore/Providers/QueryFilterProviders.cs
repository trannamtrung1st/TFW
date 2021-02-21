using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using TFW.Framework.EFCore.Context;
using TFW.Framework.EFCore.Options;

namespace TFW.Framework.EFCore.Providers
{
    public interface IQueryFilterConfigProvider
    {
        /// <summary>
        /// Expression provider signature: Expression<Func<E, bool>> Method<E>(TDBContext dbContext) where E : class
        /// </summary>
        (Func<IMutableEntityType, bool>, string)[] Conditions { get; }
    }

    public interface IQueryFilterProvider
    {
        QueryFilter[] DefaultFilters { get; }
    }
}
