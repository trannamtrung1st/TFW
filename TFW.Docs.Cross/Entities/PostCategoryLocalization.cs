namespace TFW.Docs.Cross.Entities
{
    public class PostCategoryLocalization : AppLocalizationEntity<int, PostCategory>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
