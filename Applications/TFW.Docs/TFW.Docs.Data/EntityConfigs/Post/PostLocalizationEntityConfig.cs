using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Data.EntityConfigs
{
    public class PostLocalizationEntityConfig : BaseLocalizationEntityConfig<PostLocalizationEntity, int, PostEntity>
    {
        public override void Configure(EntityTypeBuilder<PostLocalizationEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title).IsRequired();

            builder.Property(e => e.PostIndex)
                .HasMaxLength(PostLocalizationEntity.PostIndexMaxLength)
                .IsRequired();
        }
    }
}
