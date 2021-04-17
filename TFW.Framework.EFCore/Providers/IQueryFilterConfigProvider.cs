using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace TFW.Framework.EFCore.Providers
{
    public interface IQueryFilterConfigProvider
    {
        /// <summary>
        /// Expression provider signature: Expression<Func<E, bool>> Method<E>(TDBContext dbContext) where E : class
        /// </summary>
        IEnumerable<(Func<IMutableEntityType, bool>, string)> Conditions { get; }
    }
}
