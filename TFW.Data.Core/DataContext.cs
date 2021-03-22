using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross;
using TFW.Cross.Entities;
using TFW.Framework.Data;
using TFW.Framework.DI.Attributes;
using TFW.Framework.EFCore.Context;
using TFW.Framework.EFCore.Helpers;
using TFW.Framework.EFCore.Options;

namespace TFW.Data.Core
{
    [ProviderService(ServiceLifetime.Scoped, ServiceType = typeof(DbContext))]
    [ProviderService(ServiceLifetime.Scoped, ServiceType = typeof(IFullAuditableDbContext))]
    [ProviderService(ServiceLifetime.Scoped, ServiceType = typeof(IBaseDbContext))]
    [ProviderService(ServiceLifetime.Scoped, ServiceType = typeof(IHighLevelDbContext))]
    public partial class DataContext : BaseIdentityDbContext<AppUser, AppRole, string, IdentityUserClaim<string>,
        AppUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly IDbConnectionPoolManager _poolManager;
        private bool disposedValue;

        public DataContext(IDbConnectionPoolManager poolManager = null) : base()
        {
            _poolManager = poolManager;
        }

        public DataContext(QueryFilterOptions queryFilterOptions,
            IDbConnectionPoolManager poolManager = null) : base(queryFilterOptions)
        {
            _poolManager = poolManager;
        }

        public DataContext(DbContextOptions options,
            IOptionsSnapshot<QueryFilterOptions> queryFilterOptions = null,
            IDbConnectionPoolManager poolManager = null) : base(options, queryFilterOptions)
        {
            _poolManager = poolManager;
        }

        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<PostCategory> PostCategory { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<TagOfPost> TagOfPost { get; set; }

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
            auditableEntity.CreatedUserId = BusinessContext.Current?.PrincipalInfo?.UserId;
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
                    softDeleteEntity.DeletedUserId = BusinessContext.Current?.PrincipalInfo?.UserId;
                    isSoftDeleted = true;
                }
            }

            if (isSoftDeleted || entity is IAppAuditableEntity == false) return;

            var auditableEntity = entity as IAppAuditableEntity;
            auditableEntity.LastModifiedUserId = BusinessContext.Current?.PrincipalInfo?.UserId;
        }

        #region Dispose
        public override void Dispose()
        {
            DisposeAsync(true).Wait();

            base.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);

            await base.DisposeAsync();
        }

        private Task DisposeAsync(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_poolManager?.IsNullObject == false)
                        _poolManager.TryReturnToPoolAsync(dbConnection);
                }

                disposedValue = true;
            }

            return Task.CompletedTask;
        }
        #endregion
    }
}
