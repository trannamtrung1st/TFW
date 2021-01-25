using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Entities
{
    public class Note : IAuditableEntity<string>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CategoryName { get; set; }

        public virtual NoteCategory Category { get; set; }
        public virtual AppUser CreatedUser { get; set; }

        // audited
        public DateTime CreatedTime { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public string LastModifiedUserId { get; set; }
    }
}