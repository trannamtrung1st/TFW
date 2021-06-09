using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Entities;
using TFW.Framework.EFCore.Extensions;

namespace TFW.Docs.Data.EntityConfigs
{
    public class PostCategoryLocalizationEntityConfig
        : BaseLocalizationEntityConfig<PostCategoryLocalizationEntity, int, PostCategoryEntity>
    {
        public override void Configure(EntityTypeBuilder<PostCategoryLocalizationEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title).IsRequired();
        }
    }
}
