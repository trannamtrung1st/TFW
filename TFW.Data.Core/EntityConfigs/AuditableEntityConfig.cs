using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Cross.Models;

namespace TFW.Data.Core.EntityConfigs
{
    public abstract class AuditableEntityConfig<T> : IEntityTypeConfiguration<T> where T : class
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            var entityType = typeof(T);

            if (typeof(IAuditableEntity<string>).IsAssignableFrom(entityType))
            {
                builder.Property(o => (o as IAuditableEntity<string>).CreatedUserId)
                    .IsUnicode(false)
                    .HasMaxLength(DataConsts.UserKeyStringLength);

                builder.Property(o => (o as IAuditableEntity<string>).LastModifiedUserId)
                    .IsUnicode(false)
                    .HasMaxLength(DataConsts.UserKeyStringLength);
            }

            if (typeof(IShallowDeleteEntity<string>).IsAssignableFrom(entityType))
            {
                builder.Property(o => (o as IShallowDeleteEntity<string>).DeletedUserId)
                    .IsUnicode(false)
                    .HasMaxLength(DataConsts.UserKeyStringLength);
            }
        }
    }
}
