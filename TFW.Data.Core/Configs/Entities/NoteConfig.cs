using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;

namespace TFW.Data.Core.Configs.Entities
{
    public class NoteConfig : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.Property(e => e.Title).IsRequired()
                .HasMaxLength(255);
         
            builder.Property(e => e.Content)
                .IsUnicode();
            
            builder.HasOne(e => e.CreatedUser)
                .WithMany(e => e.Notes)
                .HasForeignKey(e => e.CreatedUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Note_AppUser");
            
            builder.HasOne(e => e.Category)
                .WithMany(e => e.Notes)
                .HasForeignKey(e => e.CategoryName)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Note_Category");
        }
    }
}
