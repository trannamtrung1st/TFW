using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
