using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Data.EntityConfigs
{
    public class PostCategoryEntityConfig : BaseLocalizedEntityConfig<PostCategoryEntity, PostCategoryLocalizationEntity>
    {
        public override void Configure(EntityTypeBuilder<PostCategoryEntity> builder)
        {
            base.Configure(builder);

            builder.HasOne(e => e.StartingPost)
                .WithMany()
                .HasForeignKey(e => e.StartingPostId);
        }
    }
}
