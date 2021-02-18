using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Cross.Models
{
    public interface IAuditableEntity<TUserKey>
    {
        public DateTime CreatedTime { get; set; }
        public TUserKey CreatedUserId { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public TUserKey LastModifiedUserId { get; set; }
    }

    public interface IShallowDeleteEntity<TUserKey>
    {
        public DateTime DeletedTime { get; set; }
        public TUserKey DeletedUserId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public abstract class AuditableEntity<TUserKey> : IAuditableEntity<TUserKey>
    {
        public virtual DateTime CreatedTime { get; set; }
        public virtual TUserKey CreatedUserId { get; set; }
        public virtual DateTime LastModifiedTime { get; set; }
        public virtual TUserKey LastModifiedUserId { get; set; }
    }

    public abstract class ShallowDeleteEntity<TUserKey> : IShallowDeleteEntity<TUserKey>
    {
        public virtual DateTime DeletedTime { get; set; }
        public virtual TUserKey DeletedUserId { get; set; }
        public virtual bool IsDeleted { get; set; }
    }

    public abstract class FullAuditableEntity<TUserKey> : IAuditableEntity<TUserKey>, IShallowDeleteEntity<TUserKey>
    {
        public virtual DateTime DeletedTime { get; set; }
        public virtual TUserKey DeletedUserId { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        public virtual TUserKey CreatedUserId { get; set; }
        public virtual DateTime LastModifiedTime { get; set; }
        public virtual TUserKey LastModifiedUserId { get; set; }
    }
}
