using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Data.EntityConfigs
{
    public class PostCategoryEntityConfig : BaseEntityConfig<PostCategory>
    {
        public override void Configure(EntityTypeBuilder<PostCategory> builder)
        {
            base.Configure(builder);

            builder.HasOne(e => e.StartingPost)
                .WithMany()
                .HasForeignKey(e => e.StartingPostId);
        }
    }
}
