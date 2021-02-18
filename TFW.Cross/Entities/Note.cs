using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Entities
{
    public class Note : AppAuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CategoryName { get; set; }

        public virtual NoteCategory Category { get; set; }
        public virtual AppUser CreatedUser { get; set; }
    }
}