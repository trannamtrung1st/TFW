using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Entities
{
    public class NoteCategory : AppShallowDeleteEntity
    {
        public NoteCategory()
        {
            Notes = new List<Note>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual IList<Note> Notes { get; set; }
    }
}
