using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Entities
{
    public class AppRole : IdentityRole<string>
    {
        public AppRole()
        {
            UserRoles = new List<AppUserRole>();
        }

        public virtual IList<AppUserRole> UserRoles { get; set; }
    }
}
