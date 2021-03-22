using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Entities
{
    public class PostCategory : AppFullAuditableEntity
    {
        public PostCategory()
        {
            Posts = new List<Post>();
        }

        public string Name { get; set; }
        public int? Order { get; set; }
        public string Description { get; set; }
        public string ParentCategory { get; set; }
        public int Level { get; set; }

        public virtual PostCategory Parent { get; set; }
        public virtual AppUser Creator { get; set; }
        public virtual IList<Post> Posts { get; set; }
        public virtual IList<PostCategory> Children { get; set; }
    }
}
