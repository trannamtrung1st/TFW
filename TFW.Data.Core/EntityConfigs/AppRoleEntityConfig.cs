using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross;
using TFW.Cross.Entities;

namespace TFW.Data.Core.EntityConfigs
{
    public class AppRoleEntityConfig : BaseEntityConfig<AppRole>
    {
        public override void Configure(EntityTypeBuilder<AppRole> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Id)
                .IsUnicode(false)
                .HasMaxLength(100);

            builder.HasData(new[]
            {
                new AppRole
                {
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Name = RoleName.Administrator,
                    NormalizedName = RoleName.Administrator.ToUpper(),
                    Id = RoleName.Administrator
                }
            });
        }
    }
}
