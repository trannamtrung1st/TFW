using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Entities
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
}
