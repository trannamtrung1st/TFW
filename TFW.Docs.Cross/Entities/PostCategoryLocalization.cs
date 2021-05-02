namespace TFW.Docs.Cross.Entities
{
    public class PostCategoryLocalization : AppLocalizationEntity<int, PostCategory>
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
