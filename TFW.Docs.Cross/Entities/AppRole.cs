using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.Cross.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole()
        {
            UserRoles = new HashSet<AppUserRole>();
        }

        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}
