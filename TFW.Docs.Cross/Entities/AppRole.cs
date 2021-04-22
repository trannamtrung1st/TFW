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
            UserRoles = new List<AppUserRole>();
        }

        public virtual IList<AppUserRole> UserRoles { get; set; }
    }
}
