using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Entities
{
    public class Tag
    {
        public Tag()
        {
            TagOfPosts = new List<TagOfPost>();
        }

        public string Label { get; set; }
        public string Description { get; set; }

        public virtual IList<TagOfPost> TagOfPosts { get; set; }
    }
}
