using Microsoft.AspNetCore.Identity;

namespace TFW.Docs.Cross.Entities
{
    public class AppUserRoleEntity : IdentityUserRole<int>
    {
        public virtual AppUserEntity User { get; set; }
        public virtual AppRoleEntity Role { get; set; }
    }
}
