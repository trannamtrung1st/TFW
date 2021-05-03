using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Entities;
using TFW.Framework.EFCore.Extensions;

namespace TFW.Docs.Data.EntityConfigs
{
    public class PostCategoryEntityConfig : BaseLocalizedEntityConfig<PostCategoryEntity, int, PostCategoryLocalizationEntity>
    {
        public override void Configure(EntityTypeBuilder<PostCategoryEntity> builder)
        {
            base.Configure(builder);

            builder.HasOne(e => e.StartingPost)
                .WithMany()
                .HasForeignKey(e => e.StartingPostId);
        }
    }
}
