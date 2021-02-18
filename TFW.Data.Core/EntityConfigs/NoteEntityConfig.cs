using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;

namespace TFW.Data.Core.EntityConfigs
{
    public class NoteEntityConfig : AuditableEntityConfig<Note>
    {
        public override void Configure(EntityTypeBuilder<Note> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title).IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Content)
                .IsUnicode();

            builder.HasOne(e => e.CreatedUser)
                .WithMany(e => e.Notes)
                .HasForeignKey(e => e.CreatedUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Note_AppUser_CreatedUserId");

            builder.HasOne(e => e.Category)
                .WithMany(e => e.Notes)
                .HasForeignKey(e => e.CategoryName)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Note_Category_CategoryName");
        }
    }
}
