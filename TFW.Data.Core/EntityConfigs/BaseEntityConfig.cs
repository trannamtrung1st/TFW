using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Cross.Models;
using TFW.Framework.EFCore.Extensions;

namespace TFW.Data.Core.EntityConfigs
{
    public abstract class BaseEntityConfig<T> : IEntityTypeConfiguration<T> where T : class
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ConfigureAuditableEntity(
                userKeyStringLength: EntityConfigConsts.UserKeyStringLength,
                isUnicode: false);
        }
    }
}
