namespace TFW.Docs.Cross.Entities
{
    public class Post : AppLocalizedEntity<PostLocalization>
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int CategoryId { get; set; }

        public virtual Post Parent { get; set; }
        public virtual PostCategory Category { get; set; }
    }
}
