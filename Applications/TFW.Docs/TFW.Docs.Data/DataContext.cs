using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Entities;
using TFW.Docs.Cross.Providers;
using TFW.Framework.Common.Helpers;
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
    public partial class DataContext : BaseIdentityDbContext<AppUserEntity, AppRoleEntity, int, IdentityUserClaim<int>,
        AppUserRoleEntity, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private const string IdentityNamespace = "Microsoft.AspNetCore.Identity";
        private const string IdentityModelNameStart = "Identity";
        private const string EntityPostfix = "Entity";

        private readonly IBusinessContextProvider _businessContextProvider;
        private IMutableModel _model;

        public DataContext() : base()
        {
        }

        public DataContext(QueryFilterOptions queryFilterOptions) : base(queryFilterOptions)
        {
        }

        public DataContext(DbContextOptions options,
            IOptionsSnapshot<QueryFilterOptions> queryFilterOptions = null,
            IBusinessContextProvider businessContextProvider = null,
            AppEntitySchema entitySchema = null) : base(options, queryFilterOptions)
        {
            _businessContextProvider = businessContextProvider;

            if (_model != null) entitySchema.InitSchema(_model.ParseSchema());
        }


        public DbSet<PostCategoryEntity> PostCategory { get; set; }
        public DbSet<PostCategoryLocalizationEntity> PostCategoryLocalization { get; set; }
        public DbSet<PostEntity> Post { get; set; }
        public DbSet<PostLocalizationEntity> PostLocalization { get; set; }

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
            var modelAssembly = typeof(NamespaceModel).Assembly;

            // we can pass a 'predicate' to these below extensions to filter which type to apply

            modelBuilder.ApplyConfigurationsFromAssembly(dataContextAssembly);

            modelBuilder.RestrictDeleteBehaviour();

            //modelBuilder.UseEntityTypeNameForTable();

            modelBuilder.AddGlobalQueryFilter(this, new[] { dataContextAssembly });

            #region Restrict string length
            var identityModelTypes = ReflectionHelper.GetTypesOfNamespace(IdentityNamespace, typeof(IdentityUser).Assembly)
                .Where(t => t.Name.StartsWith(IdentityModelNameStart) && t.IsClass && !t.IsAbstract).ToArray();

            modelBuilder.RestrictStringLength(EntityConfigConsts.DefaultTitleLikeStringLength,
                extraColumnPredicate: col =>
                {
                    var colName = col.GetColumnName();
                    return !col.IsAnyPropertyOfTypes(identityModelTypes) && EntityConfigConsts
                        .CommonTitleLikeColumnEndWiths.Any(o => colName.EndsWith(o));
                });

            modelBuilder.RestrictStringLength(EntityConfigConsts.DefaultCodeLikeStringLength,
                extraColumnPredicate: col =>
                {
                    var colName = col.GetColumnName();
                    return !col.IsAnyPropertyOfTypes(identityModelTypes) && EntityConfigConsts
                        .CommonCodeLikeColumnEndWiths.Any(o => colName.EndsWith(o));
                });

            modelBuilder.RestrictStringLength(EntityConfigConsts.DefaultDescriptionLikeStringLength,
                extraColumnPredicate: col =>
                {
                    var colName = col.GetColumnName();
                    return !col.IsAnyPropertyOfTypes(identityModelTypes) && EntityConfigConsts
                        .CommonDescriptionLikeColumnEndWiths.Any(o => colName.EndsWith(o));
                });
            #endregion

            modelBuilder.AdjustTableName(eType =>
            {
                var tblName = eType.GetTableName();
                var postfixIdx = tblName.LastIndexOf(EntityPostfix);
                return postfixIdx > 0 ? tblName.Substring(0, postfixIdx) : tblName;

            }, entityTypePredicate: type => type.ClrType?.Assembly == modelAssembly);

            _model = modelBuilder.Model;
        }

        public override void PrepareAdd(object entity)
        {
            base.PrepareAdd(entity);

            if (entity is IAppAuditableEntity auditableEntity)
            {
                auditableEntity.CreatedUserId = _businessContextProvider?.BusinessContext?.PrincipalInfo?.UserId;
            }
        }

        public override void PrepareModify(object entity)
        {
            base.PrepareModify(entity);

            var isSoftDeleted = false;

            if (entity is IAppSoftDeleteEntity softDeleteEntity && softDeleteEntity.IsDeleted)
            {
                softDeleteEntity.DeletedUserId = _businessContextProvider?.BusinessContext?.PrincipalInfo?.UserId;
                isSoftDeleted = true;
            }

            if (!isSoftDeleted && entity is IAppAuditableEntity auditableEntity)
            {
                auditableEntity.LastModifiedUserId = _businessContextProvider?.BusinessContext?.PrincipalInfo?.UserId;
            }
        }

        public override EntityEntry<E> Remove<E>(E entity, bool isPhysical = false)
        {
            return base.Remove(entity, isPhysical);
        }
    }
}
