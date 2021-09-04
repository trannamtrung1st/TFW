using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Data.EntityConfigs
{
    public class AppUserEntityConfig : BaseEntityConfig<AppUserEntity>
    {
        public override void Configure(EntityTypeBuilder<AppUserEntity> builder)
        {
            base.Configure(builder);
        }
    }
}
