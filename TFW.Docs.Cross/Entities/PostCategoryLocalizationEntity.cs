namespace TFW.Docs.Cross.Entities
{
    public class PostCategoryLocalizationEntity : AppLocalizationEntity<int, PostCategoryEntity>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
