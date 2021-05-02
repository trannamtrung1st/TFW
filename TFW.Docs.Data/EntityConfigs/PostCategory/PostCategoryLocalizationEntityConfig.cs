using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Data.EntityConfigs
{
    public class PostCategoryLocalizationEntityConfig : BaseEntityConfig<PostCategoryLocalization>
    {
        public override void Configure(EntityTypeBuilder<PostCategoryLocalization> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title).IsRequired();

            builder.HasOne(e => e.Entity)
                .WithMany(e => e.ListOfLocalization)
                .HasForeignKey(e => e.EntityId);
        }
    }
}
