using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Framework.Common.Helpers;
using TFW.Framework.Cross.Models;

namespace TFW.Framework.EFCore.Helpers
{
    public static class EntityConfigHelper
    {
        public static EntityTypeBuilder<T> ConfigureAuditableEntity<T>(this EntityTypeBuilder<T> builder,
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

        public static ModelBuilder UseEntityTypeNameForTable(this ModelBuilder builder,
            Func<IMutableEntityType, bool> predicate = null)
        {
            var entityTypes = builder.Model.GetEntityTypes();

            if (predicate != null)
                entityTypes = entityTypes.Where(predicate);

            foreach (var entityType in entityTypes)
            {
                // skip Shadow types like Identity... (IdentityFramework)
                if (entityType.ClrType != null)
                    entityType.SetTableName(entityType.ClrType.Name);
            }

            return builder;
        }

        public static ModelBuilder RestrictStringLength(this ModelBuilder builder,
            int maxLength, bool? setIsFixedLength = null,
            bool unboundNormalColumnsOnly = true,
            bool identityModelPropsExcluded = true,
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

                    if (identityModelPropsExcluded)
                        strProps = strProps.Where(o => !o.IsIdentityModelDefaultProperty());

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

        public static bool IsIdentityModelDefaultProperty(this IMutableProperty prop)
        {
            var entityType = prop.DeclaringEntityType;

            var modelName = entityType.IsIdentityModel();

            return modelName != null;
        }

        public static string IsIdentityModel(this IMutableEntityType eType)
        {
            var clrType = eType.ClrType;
            do
            {
                var modelName = clrType.IsIdentityModel();

                if (modelName != null) return modelName;

                clrType = clrType.BaseType;
            }
            while (clrType != typeof(object));

            return null;
        }

        public static string IsIdentityModel(this Type type)
        {
            var modelName = Caching.IdentityEntityTypeNames.FirstOrDefault(o =>
                o == type.GetNameWithoutGenericParameters());

            return modelName;
        }

        // include string foreign keys (also unbound length)
        public static bool IsUnboundLength(this IMutableProperty prop)
        {
            return prop.GetMaxLength() == null;
        }

    }
}
