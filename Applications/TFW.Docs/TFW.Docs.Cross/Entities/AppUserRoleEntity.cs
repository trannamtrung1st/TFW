using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.Cross.Entities
{
    public class AppUserRoleEntity : IdentityUserRole<int>
    {
        public virtual AppUserEntity User { get; set; }
        public virtual AppRoleEntity Role { get; set; }
    }
}
