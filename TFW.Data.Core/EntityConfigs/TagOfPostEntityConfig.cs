using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;

namespace TFW.Data.Core.EntityConfigs
{
    public class TagOfPostEntityConfig : BaseEntityConfig<TagOfPost>
    {
        public override void Configure(EntityTypeBuilder<TagOfPost> builder)
        {
            base.Configure(builder);

            builder.HasKey(e => new { e.PostId, e.TagLabel });

            builder.HasOne(e => e.Tag)
                .WithMany(e => e.TagOfPosts)
                .HasForeignKey(e => e.TagLabel)
                .HasConstraintName("FK_TagOfPost_Tag_TagLabel");

            builder.HasOne(e => e.Post)
                .WithMany(e => e.TagOfPosts)
                .HasForeignKey(e => e.PostId)
                .HasConstraintName("FK_TagOfPost_Post_PostId");
        }
    }
}
