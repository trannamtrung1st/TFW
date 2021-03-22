using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;
using TFW.Framework.Data;

namespace TFW.Data.Core.EntityConfigs
{
    public class PostEntityConfig : BaseEntityConfig<Post>
    {
        public override void Configure(EntityTypeBuilder<Post> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title)
                .HasMaxLength(255);

            builder.Property(e => e.Description)
                .HasMaxLength(1000);

            builder.Property(e => e.PostContent)
                .HasColumnType(SqlServerColumnType.ntext);

            builder.HasOne(e => e.Category)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.CategoryName)
                .HasConstraintName("FK_Post_PostCategory_CategoryName");

            builder.HasOne(e => e.Creator)
                .WithMany(e => e.CreatedPosts)
                .HasForeignKey(e => e.CreatedUserId)
                .HasConstraintName("FK_Post_AppUser_CreatedUserId");
        }
    }
}
