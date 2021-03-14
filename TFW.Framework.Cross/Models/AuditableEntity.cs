using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Cross.Models
{
    public interface IAuditableEntity
    {
        public DateTime CreatedTime { get; set; }
        public DateTime? LastModifiedTime { get; set; }
    }

    public interface IAuditableEntity<TUserKey> : IAuditableEntity
    {
        public TUserKey CreatedUserId { get; set; }
        public TUserKey LastModifiedUserId { get; set; }
    }

    public interface ISoftDeleteEntity
    {
        public DateTime? DeletedTime { get; set; }
        public bool IsDeleted { get; set; }

    }

    public interface ISoftDeleteEntity<TUserKey> : ISoftDeleteEntity
    {
        public TUserKey DeletedUserId { get; set; }
    }

    public abstract class AuditableEntity<TUserKey> : IAuditableEntity<TUserKey>
    {
        public virtual DateTime CreatedTime { get; set; }
        public virtual TUserKey CreatedUserId { get; set; }
        public virtual DateTime? LastModifiedTime { get; set; }
        public virtual TUserKey LastModifiedUserId { get; set; }
    }

    public abstract class SoftDeleteEntity<TUserKey> : ISoftDeleteEntity<TUserKey>
    {
        public virtual DateTime? DeletedTime { get; set; }
        public virtual TUserKey DeletedUserId { get; set; }
        public virtual bool IsDeleted { get; set; }
    }

    public abstract class FullAuditableEntity<TUserKey> : IAuditableEntity<TUserKey>, ISoftDeleteEntity<TUserKey>
    {
        public virtual DateTime? DeletedTime { get; set; }
        public virtual TUserKey DeletedUserId { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        public virtual TUserKey CreatedUserId { get; set; }
        public virtual DateTime? LastModifiedTime { get; set; }
        public virtual TUserKey LastModifiedUserId { get; set; }
    }
}
