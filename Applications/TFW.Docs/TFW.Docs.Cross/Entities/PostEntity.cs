using System.Collections.Generic;

namespace TFW.Docs.Cross.Entities
{
    public class PostEntity : AppLocalizedEntity<PostLocalizationEntity>
    {
        public PostEntity() : base()
        {
            SubPosts = new HashSet<PostEntity>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int CategoryId { get; set; }

        public virtual PostEntity Parent { get; set; }
        public virtual PostCategoryEntity Category { get; set; }
        public virtual ICollection<PostEntity> SubPosts { get; set; }
    }
}
