using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Cross.i18n;
using TFW.Framework.EFCore.Extensions;

namespace TFW.Docs.Data.EntityConfigs
{
    public abstract class BaseEntityConfig<T> : IEntityTypeConfiguration<T> where T : class
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
        }
    }

    public abstract class BaseLocalizationEntityConfig<T, EKey, TEntity> : BaseEntityConfig<T>
        where T : class, ILocalizationEntity<EKey, TEntity>
        where TEntity : class
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.ConfigureLocalizationEntity<T, EKey, TEntity>();
        }
    }

    public abstract class BaseLocalizedEntityConfig<T, LEntity> : BaseEntityConfig<T>
        where T : class, ILocalizedEntity<LEntity>
        where LEntity : class, ILocalizationEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.ConfigureLocalizedEntity<T, LEntity>();
        }
    }
}
