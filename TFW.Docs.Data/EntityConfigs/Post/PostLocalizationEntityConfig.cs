using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Data.EntityConfigs
{
    public class PostLocalizationEntityConfig : BaseEntityConfig<PostLocalization>
    {
        public override void Configure(EntityTypeBuilder<PostLocalization> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title).IsRequired();

            builder.Property(e => e.PostIndex)
                .HasMaxLength(PostLocalization.PostIndexMaxLength)
                .IsRequired();

            builder.HasOne(e => e.Entity)
                .WithMany(e => e.ListOfLocalization)
                .HasForeignKey(e => e.EntityId);
        }
    }
}
