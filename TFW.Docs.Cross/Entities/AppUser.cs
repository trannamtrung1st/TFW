using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.Cross.Entities
{
    public abstract class FullAuditableUser : IdentityUser<int>,
        IAppAuditableEntity, IAppSoftDeleteEntity
    {
        public DateTime CreatedTime { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public int? LastModifiedUserId { get; set; }
        public DateTime? DeletedTime { get; set; }
        public int? DeletedUserId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AppUser : FullAuditableUser
    {
        public AppUser()
        {
            UserRoles = new List<AppUserRole>();
        }

        public string FullName { get; set; }

        public virtual IList<AppUserRole> UserRoles { get; set; }
    }
}
