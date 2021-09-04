using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Data.EntityConfigs
{
    public class PostCategoryLocalizationEntityConfig
        : BaseLocalizationEntityConfig<PostCategoryLocalizationEntity, int, PostCategoryEntity>
    {
        public override void Configure(EntityTypeBuilder<PostCategoryLocalizationEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title).IsRequired();
        }
    }
}
