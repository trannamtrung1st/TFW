using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Data.EntityConfigs
{
    public class PostEntityConfig : BaseLocalizedEntityConfig<PostEntity, PostLocalizationEntity>
    {
        public override void Configure(EntityTypeBuilder<PostEntity> builder)
        {
            base.Configure(builder);

            builder.HasOne(e => e.Parent)
                .WithMany(e => e.SubPosts)
                .HasForeignKey(e => e.ParentId);

            builder.HasOne(e => e.Category)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.CategoryId);
        }
    }
}
