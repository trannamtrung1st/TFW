using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;

namespace TFW.Data.Core.EntityConfigs
{
    public class TagEntityConfig : BaseEntityConfig<Tag>
    {
        public override void Configure(EntityTypeBuilder<Tag> builder)
        {
            base.Configure(builder);

            builder.HasKey(e => e.Label);

            builder.Property(e => e.Label)
                .IsUnicode(false)
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(1000);
        }
    }
}
