using System.Collections;
using System.Collections.Generic;

namespace TFW.Docs.Cross.Entities
{
    public class PostCategoryEntity : AppLocalizedEntity<int, PostCategoryLocalizationEntity>
    {
        public PostCategoryEntity() : base()
        {
            Posts = new HashSet<PostEntity>();
        }

        public int Id { get; set; }
        public int? StartingPostId { get; set; }

        public virtual PostEntity StartingPost { get; set; }
        public virtual ICollection<PostEntity> Posts { get; set; }
    }
}
