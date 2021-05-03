using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.Cross.Entities
{
    public class AppRoleEntity : IdentityRole<int>
    {
        public AppRoleEntity()
        {
            UserRoles = new HashSet<AppUserRoleEntity>();
        }

        public virtual ICollection<AppUserRoleEntity> UserRoles { get; set; }
    }
}
