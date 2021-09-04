using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TFW.Docs.Cross.Entities
{
    public abstract class FullAuditableUser : IdentityUser<int>,
        IAppAuditableEntity, IAppSoftDeleteEntity
    {
        public DateTimeOffset CreatedTime { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTimeOffset? LastModifiedTime { get; set; }
        public int? LastModifiedUserId { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public int? DeletedUserId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AppUserEntity : FullAuditableUser
    {
        public AppUserEntity()
        {
            UserRoles = new HashSet<AppUserRoleEntity>();
        }

        public string FullName { get; set; }

        public virtual ICollection<AppUserRoleEntity> UserRoles { get; set; }
    }
}
