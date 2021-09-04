using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Data.EntityConfigs
{
    public class AppUserRoleEntityConfig : BaseEntityConfig<AppUserRoleEntity>
    {
        public override void Configure(EntityTypeBuilder<AppUserRoleEntity> builder)
        {
            base.Configure(builder);

            builder.HasOne(e => e.User)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(e => e.UserId);
            //.OnDelete(DeleteBehavior.Restrict)

            builder.HasOne(e => e.Role)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(e => e.RoleId);
            //.OnDelete(DeleteBehavior.Restrict)
        }
    }
}
