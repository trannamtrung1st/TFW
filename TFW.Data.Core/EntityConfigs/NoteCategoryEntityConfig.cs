using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;

namespace TFW.Data.Core.EntityConfigs
{
    public class NoteCategoryEntityConfig : BaseEntityConfig<NoteCategory>
    {
        public override void Configure(EntityTypeBuilder<NoteCategory> builder)
        {
            base.Configure(builder);

            builder.HasKey(e => e.Name);

            builder.Property(e => e.Name).IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Description)
                .HasMaxLength(1000);
        }
    }
}
