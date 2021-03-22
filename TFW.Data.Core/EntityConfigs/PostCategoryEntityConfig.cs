using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;

namespace TFW.Data.Core.EntityConfigs
{
    public class PostCategoryEntityConfig : BaseEntityConfig<PostCategory>
    {
        public override void Configure(EntityTypeBuilder<PostCategory> builder)
        {
            base.Configure(builder);

            builder.HasKey(e => e.Name);

            builder.Property(e => e.Name)
                .HasMaxLength(255);

            builder.Property(e => e.Description)
                .HasMaxLength(1000);

            builder.HasOne(e => e.Creator)
                .WithMany(e => e.CreatedCategories)
                .HasForeignKey(e => e.CreatedUserId)
                .HasConstraintName("FK_PostCategory_AppUser_CreatedUserId");

            builder.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentCategory)
                .HasConstraintName("FK_PostCategory_ParentCategory");
        }
    }
}
