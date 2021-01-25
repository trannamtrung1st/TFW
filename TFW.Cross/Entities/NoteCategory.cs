using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Entities
{
    public class NoteCategory : IShallowDeleteEntity<string>
    {
        public NoteCategory()
        {
            Notes = new List<Note>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual IList<Note> Notes { get; set; }

        // audited
        public DateTime DeletedTime { get; set; }
        public string DeletedUserId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
