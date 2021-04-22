using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Data.EntityConfigs
{
    public class AppRoleEntityConfig : BaseEntityConfig<AppRole>
    {
        public override void Configure(EntityTypeBuilder<AppRole> builder)
        {
            base.Configure(builder);

            var listRole = new List<AppRole>
            {
                new AppRole
                {
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Name = RoleName.Administrator,
                    NormalizedName = RoleName.Administrator.ToUpper(),
                }
            };

            for (var i = 0; i < listRole.Count; i++)
                listRole[i].Id = i + 1;

            builder.HasData(listRole);
        }
    }
}
