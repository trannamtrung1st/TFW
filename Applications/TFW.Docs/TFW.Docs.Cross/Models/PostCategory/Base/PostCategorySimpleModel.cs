using Newtonsoft.Json;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class PostCategorySimpleModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
