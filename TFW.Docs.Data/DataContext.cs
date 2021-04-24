using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using TFW.Docs.Cross.Entities;
using TFW.Docs.Cross.Providers;
using TFW.Framework.DI.Attributes;
using TFW.Framework.EFCore.Context;
using TFW.Framework.EFCore.Extensions;
using TFW.Framework.EFCore.Options;

namespace TFW.Docs.Data
{
    [ProviderService(ServiceLifetime.Scoped, ServiceType = typeof(DbContext))]
    [ProviderService(ServiceLifetime.Scoped, ServiceType = typeof(IFullAuditableDbContext))]
    [ProviderService(ServiceLifetime.Scoped, ServiceType = typeof(IBaseDbContext))]
    [ProviderService(ServiceLifetime.Scoped, ServiceType = typeof(IHighLevelDbContext))]
    public partial class DataContext : BaseIdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>,
        AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        [Inject(Required = false)]
        public IBusinessContextProvider BusinessContextProvider { get; set; }

        public DataContext() : base()
        {
        }

        public DataContext(QueryFilterOptions queryFilterOptions) : base(queryFilterOptions)
        {
        }

        public DataContext(DbContextOptions options,
            IOptionsSnapshot<QueryFilterOptions> queryFilterOptions = null) : base(options, queryFilterOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable(DataConsts.ConnStrKey));
                //require Microsoft.EntityFrameworkCore.Proxies
                //.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var dataContextAssembly = typeof(DataContext).Assembly;

            // we can pass a 'predicate' to these below extensions to filter which type to apply

            modelBuilder.ApplyConfigurationsFromAssembly(dataContextAssembly);

            modelBuilder.RestrictDeleteBehaviour();

            //modelBuilder.UseEntityTypeNameForTable();

            modelBuilder.AddGlobalQueryFilter(this, new[] { dataContextAssembly });

            modelBuilder.RestrictStringLength(EntityConfigConsts.DefaultTitleLikeStringLength,
                extraColumnPredicate: col =>
                {
                    var colName = col.GetColumnName();
                    return EntityConfigConsts
                        .CommonTitleLikeColumnEndWiths.Any(o => colName.EndsWith(o));
                });

            modelBuilder.RestrictStringLength(EntityConfigConsts.DefaultCodeLikeStringLength,
                extraColumnPredicate: col =>
                {
                    var colName = col.GetColumnName();
                    return EntityConfigConsts
                        .CommonCodeLikeColumnEndWiths.Any(o => colName.EndsWith(o));
                });

            modelBuilder.RestrictStringLength(EntityConfigConsts.DefaultDescriptionLikeStringLength,
                extraColumnPredicate: col =>
                {
                    var colName = col.GetColumnName();
                    return EntityConfigConsts
                        .CommonDescriptionLikeColumnEndWiths.Any(o => colName.EndsWith(o));
                });

            Framework.EFCore.Caching.ClearCache();
        }

        public override void PrepareAdd(object entity)
        {
            base.PrepareAdd(entity);

            if (entity is IAppAuditableEntity == false) return;

            var auditableEntity = entity as IAppAuditableEntity;
            auditableEntity.CreatedUserId = BusinessContextProvider?.BusinessContext?.PrincipalInfo?.UserId;
        }

        public override void PrepareModify(object entity)
        {
            base.PrepareModify(entity);

            var isSoftDeleted = false;

            if (entity is IAppSoftDeleteEntity)
            {
                var softDeleteEntity = entity as IAppSoftDeleteEntity;

                if (softDeleteEntity.IsDeleted)
                {
                    softDeleteEntity.DeletedUserId = BusinessContextProvider?.BusinessContext?.PrincipalInfo?.UserId;
                    isSoftDeleted = true;
                }
            }

            if (isSoftDeleted || entity is IAppAuditableEntity == false) return;

            var auditableEntity = entity as IAppAuditableEntity;
            auditableEntity.LastModifiedUserId = BusinessContextProvider?.BusinessContext?.PrincipalInfo?.UserId;
        }
    }
}
