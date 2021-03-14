using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;

namespace TFW.Data.Core.EntityConfigs
{
    public class AppUserRoleEntityConfig : BaseEntityConfig<AppUserRole>
    {
        public override void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            base.Configure(builder);

            builder.HasOne(e => e.User)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(e => e.UserId)
                //.OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_AppUserRole_AppUser_UserId");

            builder.HasOne(e => e.Role)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(e => e.RoleId)
                //.OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_AppUserRole_AppRole_RoleId");
        }
    }
}
