namespace TFW.Docs.Cross.Entities
{
    public class PostLocalizationEntity : AppLocalizationEntity<int, PostEntity>
    {
        public const int PostIndexMaxLength = 256;

        public int Id { get; set; }
        public string PostIndex { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
