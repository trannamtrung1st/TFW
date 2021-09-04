using System;

namespace TFW.Framework.Cross.Audit
{
    public interface IAuditableEntity
    {
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastModifiedTime { get; set; }
    }

    public interface IAuditableEntity<TUserKey> : IAuditableEntity
    {
        public TUserKey CreatedUserId { get; set; }
        public TUserKey LastModifiedUserId { get; set; }
    }

    public interface ISoftDeleteEntity
    {
        public DateTimeOffset? DeletedTime { get; set; }
        public bool IsDeleted { get; set; }

    }

    public interface ISoftDeleteEntity<TUserKey> : ISoftDeleteEntity
    {
        public TUserKey DeletedUserId { get; set; }
    }

    public abstract class AuditableEntity<TUserKey> : IAuditableEntity<TUserKey>
    {
        public virtual DateTimeOffset CreatedTime { get; set; }
        public virtual TUserKey CreatedUserId { get; set; }
        public virtual DateTimeOffset? LastModifiedTime { get; set; }
        public virtual TUserKey LastModifiedUserId { get; set; }
    }

    public abstract class SoftDeleteEntity<TUserKey> : ISoftDeleteEntity<TUserKey>
    {
        public virtual DateTimeOffset? DeletedTime { get; set; }
        public virtual TUserKey DeletedUserId { get; set; }
        public virtual bool IsDeleted { get; set; }
    }

    public abstract class FullAuditableEntity<TUserKey> : IAuditableEntity<TUserKey>, ISoftDeleteEntity<TUserKey>
    {
        public virtual DateTimeOffset? DeletedTime { get; set; }
        public virtual TUserKey DeletedUserId { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DateTimeOffset CreatedTime { get; set; }
        public virtual TUserKey CreatedUserId { get; set; }
        public virtual DateTimeOffset? LastModifiedTime { get; set; }
        public virtual TUserKey LastModifiedUserId { get; set; }
    }
}
