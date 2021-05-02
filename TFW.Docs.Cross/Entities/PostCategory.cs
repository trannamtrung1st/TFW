using System.Collections;
using System.Collections.Generic;

namespace TFW.Docs.Cross.Entities
{
    public class PostCategory : AppLocalizedEntity<PostCategoryLocalization>
    {
        public PostCategory() : base()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public int? StartingPostId { get; set; }

        public virtual Post StartingPost { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
