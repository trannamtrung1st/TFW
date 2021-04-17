using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Text;
using TFW.Business.Core.Helpers;
using TFW.Cross;
using TFW.Cross.Entities;
using TFW.Framework.Cross.Models;
using TFW.Framework.EFCore.Context;
using TFW.Framework.EFCore.Extensions;
using TFW.Framework.EFCore.Options;
using TFW.Framework.EFCore.Providers;

namespace TFW.Business.Core.Providers
{
    public class AppQueryFilterProvider : IQueryFilterProvider
    {
        public IEnumerable<QueryFilter> DefaultFilters => ImmutableArray.Create(
            QueryFilter.BuildDefaultSoftDelete(),
            new QueryFilter(QueryFilterName.AnotherFilter1, applyFilter: o => o.IsAppUserEntity())
        );
    }

    public class AppQueryFilterConfigProvider : IQueryFilterConfigProvider
    {
        public IEnumerable<(Func<IMutableEntityType, bool>, string)> Conditions =>
            ImmutableArray.Create<(Func<IMutableEntityType, bool>, string)>(
                (ShouldAddSoftDeleteFilter, nameof(CreateSoftDeleteFilter))
            );

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

    // [TODO] remove !!! This below section is for demonstration only
    #region Another filter demonstration
    /// <summary>
    /// This will be add to query filter as an OR
    /// </summary>
    public class AnotherQueryFilterConfigProvider : IQueryFilterConfigProvider
    {
        public IEnumerable<(Func<IMutableEntityType, bool>, string)> Conditions =>
            ImmutableArray.Create<(Func<IMutableEntityType, bool>, string)>(
                (ShouldAddAnotherFilter1, nameof(CreateAnotherFilter1))
            );

        protected virtual bool ShouldAddAnotherFilter1(IMutableEntityType eType)
        {
            return eType.ClrType?.IsAppUserEntity() == true;
        }

        protected virtual Expression<Func<TEntity, bool>> CreateAnotherFilter1<TEntity>(
            IFullAuditableDbContext dbContext) where TEntity : AppUser
        {
            return (o) =>
                !dbContext.IsFilterAppliedForEntity(QueryFilterName.AnotherFilter1, typeof(TEntity)) ||
                    o.Id == "Never true";
        }
    }
    #endregion
}
