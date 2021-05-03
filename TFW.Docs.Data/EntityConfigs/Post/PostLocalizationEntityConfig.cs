using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Data.EntityConfigs
{
    public class PostLocalizationEntityConfig : BaseEntityConfig<PostLocalizationEntity>
    {
        public override void Configure(EntityTypeBuilder<PostLocalizationEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title).IsRequired();

            builder.Property(e => e.PostIndex)
                .HasMaxLength(PostLocalizationEntity.PostIndexMaxLength)
                .IsRequired();

            builder.HasOne(e => e.Entity)
                .WithMany(e => e.ListOfLocalization)
                .HasForeignKey(e => e.EntityId);
        }
    }
}
