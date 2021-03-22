using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Entities
{
    public class Post : AppFullAuditableEntity
    {
        public Post()
        {
            TagOfPosts = new List<TagOfPost>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PostContent { get; set; }
        public string CategoryName { get; set; }

        public virtual PostCategory Category { get; set; }
        public virtual AppUser Creator { get; set; }
        public virtual IList<TagOfPost> TagOfPosts { get; set; }
    }
}
