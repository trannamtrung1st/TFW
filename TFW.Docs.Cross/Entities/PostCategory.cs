namespace TFW.Docs.Cross.Entities
{
    public class PostCategory : AppLocalizedEntity<PostCategoryLocalization>
    {
        public PostCategory() : base()
        {
        }

        public int Id { get; set; }
        public int? StartingPostId { get; set; }

        public virtual Post StartingPost { get; set; }
    }
}
