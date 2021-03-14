using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Entities
{
    public abstract class FullAuditableUser : IdentityUser<string>,
        IAppAuditableEntity, IAppSoftDeleteEntity
    {
        public DateTime CreatedTime { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public string LastModifiedUserId { get; set; }
        public DateTime? DeletedTime { get; set; }
        public string DeletedUserId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AppUser : FullAuditableUser
    {
        public AppUser()
        {
            UserRoles = new List<AppUserRole>();
            Notes = new List<Note>();
        }

        public string FullName { get; set; }

        public virtual IList<AppUserRole> UserRoles { get; set; }
        public virtual IList<Note> Notes { get; set; }
    }
}
