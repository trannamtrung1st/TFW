using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TFW.Framework.Cross.Audit;
using TFW.Framework.EFCore.Context;
using TFW.Framework.EFCore.Extensions;
using TFW.Framework.EFCore.Options;
using TFW.Framework.EFCore.Providers;

namespace TFW.Docs.Data.Providers
{
    public class AppQueryFilterProvider : IQueryFilterProvider
    {
        public IEnumerable<QueryFilter> DefaultFilters => new[]
        {
            QueryFilter.BuildDefaultSoftDelete()
        };
    }

    public class AppQueryFilterConfigProvider : IQueryFilterConfigProvider
    {
        public IEnumerable<(Func<IMutableEntityType, bool> Predicate, string MethodName)> Conditions =>
            new (Func<IMutableEntityType, bool> Predicate, string MethodName)[]
            {
                (ShouldAddSoftDeleteFilter, nameof(CreateSoftDeleteFilter))
            };


        #region Soft delete filter
        protected virtual bool ShouldAddSoftDeleteFilter(IMutableEntityType eType)
        {
            return eType.ClrType?.IsSoftDeleteEntity() == true;
        }

        protected virtual Expression<Func<TEntity, bool>> CreateSoftDeleteFilter<TEntity>(
            IFullAuditableDbContext dbContext) where TEntity : ISoftDeleteEntity
        {
            return (o) => !dbContext.IsSoftDeleteAppliedForEntity(typeof(TEntity)) ||
                    o.IsDeleted == false;
        }
        #endregion
    }
}
