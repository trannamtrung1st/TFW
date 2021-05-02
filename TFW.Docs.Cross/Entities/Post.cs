using System.Collections;
using System.Collections.Generic;

namespace TFW.Docs.Cross.Entities
{
    public class Post : AppLocalizedEntity<PostLocalization>
    {
        public Post() : base()
        {
            SubPosts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int CategoryId { get; set; }

        public virtual Post Parent { get; set; }
        public virtual PostCategory Category { get; set; }
        public virtual ICollection<Post> SubPosts { get; set; }
    }
}
