using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using TFW.Framework.Common.Extensions;
using TFW.Framework.Common.Helpers;
using TFW.Framework.Cross.Models;
using TFW.Framework.Cross.Schema;
using TFW.Framework.EFCore.Providers;

namespace TFW.Framework.EFCore.Extensions
{
    public static class EntityConfigExtensions
    {
        public const string DefaultFkPrefix = "FK";
        public const string DefaultUniqueIdxName = "UI_EntityId_Lang_Region";

        public static EntityTypeBuilder<T> ConfigureLocalizationEntity<T, EKey, TEntity>(this EntityTypeBuilder<T> builder,
            string uniqueIdxName = null)
            where T : class, ILocalizationEntity<EKey, TEntity>
            where TEntity : class
        {
            builder.Property(o => o.Lang).HasMaxLength(2).IsUnicode(false).IsRequired();

            builder.Property(o => o.Region).HasMaxLength(2).IsUnicode(false).IsRequired();

            builder.HasOne(o => o.Entity)
                .WithMany(nameof(ILocalizedEntity<T>.ListOfLocalization))
                .HasForeignKey(o => o.EntityId);

            builder.HasIndex(o => new
            {
                o.EntityId,
                o.Lang,
                o.Region
            }).IsUnique().HasName(uniqueIdxName ?? DefaultUniqueIdxName);

            return builder;
        }

        public static EntityTypeBuilder<T> ConfigureLocalizedEntity<T, LEntity>(this EntityTypeBuilder<T> builder)
            where T : class, ILocalizedEntity<LEntity>
            where LEntity : class, ILocalizationEntity
        {
            return builder;
        }

        public static EntityTypeBuilder<T> ConfigureAuditableEntityWithStringKey<T>(this EntityTypeBuilder<T> builder,
            int? userKeyStringLength = null,
            bool isUnicode = false) where T : class
        {
            var entityType = typeof(T);

            if (typeof(IAuditableEntity<string>).IsAssignableFrom(entityType))
            {
                var createdUserId = builder.Property(o => (o as IAuditableEntity<string>).CreatedUserId)
                    .IsUnicode(isUnicode);

                if (userKeyStringLength.HasValue)
                    createdUserId.HasMaxLength(userKeyStringLength.Value);

                var lastModifiedUserId = builder.Property(o => (o as IAuditableEntity<string>).LastModifiedUserId)
                    .IsUnicode(isUnicode);

                if (userKeyStringLength.HasValue)
                    lastModifiedUserId.HasMaxLength(userKeyStringLength.Value);
            }

            if (typeof(ISoftDeleteEntity<string>).IsAssignableFrom(entityType))
            {
                var deletedUserId = builder.Property(o => (o as ISoftDeleteEntity<string>).DeletedUserId)
                    .IsUnicode(isUnicode);

                if (userKeyStringLength.HasValue)
                    deletedUserId.HasMaxLength(userKeyStringLength.Value);
            }

            return builder;
        }

        public static ModelBuilder RestrictDeleteBehaviour(this ModelBuilder builder,
            Func<IMutableEntityType, bool> predicate = null)
        {
            var entityTypes = builder.Model.GetEntityTypes();

            if (predicate != null)
                entityTypes = entityTypes.Where(predicate);

            foreach (var foreignKeys in entityTypes.SelectMany(e => e.GetForeignKeys()))
                foreignKeys.DeleteBehavior = DeleteBehavior.Restrict;

            return builder;
        }

        public static ReferenceCollectionBuilder<Sub, Ref> HasDefaultConstraintName<Sub, Ref>(
            this ReferenceCollectionBuilder<Sub, Ref> builder,
            string prefix = DefaultFkPrefix,
            string principal = null, string dependent = null, string key = null)
            where Sub : class
            where Ref : class
        {
            return builder.HasConstraintName(
                builder.Metadata.GetDefaultConstraintName(prefix, principal, dependent, key));
        }

        private static string GetDefaultConstraintName(this IMutableForeignKey metadata,
            string prefix = DefaultFkPrefix,
            string principal = null, string dependent = null, string key = null)
        {
            principal ??= metadata.PrincipalEntityType.Name;
            dependent ??= metadata.DeclaringEntityType.Name;
            key ??= metadata.GetNavigation(true).Name;

            return $"{prefix}_{dependent}_{principal}_{key}";
        }

        public static ModelBuilder UseEntityTypeNameForTable(this ModelBuilder builder,
            Func<IMutableEntityType, bool> predicate = null)
        {
            var entityTypes = builder.Model.GetEntityTypes();

            if (predicate != null)
                entityTypes = entityTypes.Where(predicate);

            foreach (var entityType in entityTypes)
            {
                // skip Shadow types
                if (entityType.ClrType != null)
                    entityType.SetTableName(entityType.ClrType.Name);
            }

            return builder;
        }

        public static ModelBuilder AdjustTableName(this ModelBuilder builder,
            Func<IMutableEntityType, string> func,
            Func<IMutableEntityType, bool> entityTypePredicate = null)
        {
            var entityTypes = builder.Model.GetEntityTypes();

            if (entityTypePredicate != null)
                entityTypes = entityTypes.Where(entityTypePredicate);

            foreach (var entityType in entityTypes)
            {
                // skip Shadow types
                if (entityType.ClrType != null)
                    entityType.SetTableName(func(entityType));
            }

            return builder;
        }

        public static ModelBuilder AddGlobalQueryFilter(this ModelBuilder builder,
            DbContext dbContext, IEnumerable<Assembly> assemblies)
        {
            if (assemblies?.Any() != true)
                throw new ArgumentNullException(nameof(assemblies));

            var filterProviders = ReflectionHelper.GetAllTypesAssignableTo(
                typeof(IQueryFilterConfigProvider), assemblies).Select(o => o.CreateInstance<IQueryFilterConfigProvider>())
                    .ToArray();

            if (!filterProviders.Any()) return builder;

            var eTypes = builder.Model.GetEntityTypes();

            foreach (var entityType in eTypes)
            {
                // skip Shadow types
                if (entityType.ClrType == null) continue;

                LambdaExpression finalExpr = null;

                foreach (var provider in filterProviders.Where(o => !o.Conditions.IsNullOrEmpty()))
                {
                    LambdaExpression andExpr = null;

                    foreach (var cond in provider.Conditions)
                    {
                        if (cond.Predicate(entityType))
                        {
                            var expr = provider.GetType().GetInstanceMethod(
                                cond.MethodName, nonPublic: true).InvokeGeneric<LambdaExpression>(
                                    provider, new[] { entityType.ClrType }, dbContext);

                            if (andExpr == null) andExpr = expr;
                            else andExpr = andExpr.And(expr);
                        }
                    }

                    if (andExpr == null) continue;

                    if (finalExpr == null) finalExpr = andExpr;
                    else finalExpr = finalExpr.Or(andExpr);
                }

                if (finalExpr != null)
                    entityType.SetQueryFilter(finalExpr);
            }

            return builder;
        }

        public static ModelBuilder RestrictStringLength(this ModelBuilder builder,
            int maxLength, bool? setIsFixedLength = null,
            bool unboundNormalColumnsOnly = true,
            Func<IMutableProperty, bool> extraColumnPredicate = null,
            Func<IMutableEntityType, bool> entityTypePredicate = null)
        {
            var entityTypes = builder.Model.GetEntityTypes();

            if (entityTypePredicate != null)
                entityTypes = entityTypes.Where(entityTypePredicate);

            foreach (var entityType in entityTypes)
            {
                // skip Shadow types
                if (entityType.ClrType != null)
                {
                    var strProps = entityType.GetProperties()
                        .Where(o => o.ClrType == typeof(string)
                            && !SqlServerConsts.TextColumnTypes.Contains(o.GetColumnType()));

                    if (unboundNormalColumnsOnly)
                        strProps = strProps.Where(IsUnboundLength)
                            .Where(o => !o.IsForeignKey());

                    if (extraColumnPredicate != null)
                        strProps = strProps.Where(extraColumnPredicate);

                    foreach (var prop in strProps)
                    {
                        prop.SetMaxLength(maxLength);

                        if (setIsFixedLength != null)
                            prop.SetIsFixedLength(setIsFixedLength);
                    }
                }
            }

            return builder;
        }

        // include string foreign keys (also unbound length)
        public static bool IsUnboundLength(this IMutableProperty prop)
        {
            return prop.GetMaxLength() == null;
        }

        public static bool IsSoftDeleteEntity(this Type type)
        {
            return typeof(ISoftDeleteEntity).IsAssignableFrom(type);
        }

        public static bool IsAnyPropertyOfTypes(this IMutableProperty prop, IEnumerable<Type> types)
        {
            var entityType = prop.DeclaringEntityType.ClrType;

            return entityType == null ||
                types.Any(t => t.IsAssignableFrom(entityType));
        }

        public static IDictionary<string, SchemaPropertyInfo> ParseSchema(this IMutableModel model)
        {
            var dict = new Dictionary<string, SchemaPropertyInfo>();
            var stringType = typeof(string);

            foreach (var type in model.GetEntityTypes())
            {
                foreach (var prop in type.GetProperties())
                {
                    if (prop.ClrType == stringType)
                    {
                        dict[$"{type.ClrType.Namespace}.{type.ClrType.Name}.{prop.Name}"] = new StringPropertyInfo
                        {
                            PropertyType = prop.ClrType,
                            StringLength = prop.GetMaxLength(),
                            IsFixLength = prop.IsFixedLength(),
                            IsUnboundLength = prop.IsUnboundLength(),
                        };
                    }
                }
            }

            return dict;
        }
    }
}
