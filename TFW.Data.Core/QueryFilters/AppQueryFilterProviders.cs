using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TFW.Framework.Cross.Models;
using TFW.Framework.EFCore.Context;
using TFW.Framework.EFCore.Helpers;
using TFW.Framework.EFCore.Options;
using TFW.Framework.EFCore.Providers;

namespace TFW.Data.Core.QueryFilters
{
    public class AppQueryFilterProvider : IQueryFilterProvider
    {
        public QueryFilter[] DefaultFilters => new[]
        {
            QueryFilter.BuildDefaultSoftDelete()
        };
    }

    public class AppQueryFilterConfigProvider : IQueryFilterConfigProvider
    {
        public (Func<IMutableEntityType, bool>, string)[] Conditions => new (Func<IMutableEntityType, bool>, string)[]
        {
            (ShouldAddSoftDeleteFilter, nameof(CreateSoftDeleteFilter))
        };

        #region Soft delete filter
        protected virtual bool ShouldAddSoftDeleteFilter(IMutableEntityType eType)
        {
            return eType.IsSoftDeleteEntity();
        }

        protected virtual Expression<Func<TEntity, bool>> CreateSoftDeleteFilter<TEntity>(
            IFullAuditableDbContext dbContext) where TEntity : class
        {
            return (TEntity o) =>
                (!dbContext.IsSoftDeleteEnabled() || !dbContext.IsSoftDeleteAppliedForEntity(typeof(TEntity)) ||
                    ((ISoftDeleteEntity)o).IsDeleted == false);
        }
        #endregion

    }
}
