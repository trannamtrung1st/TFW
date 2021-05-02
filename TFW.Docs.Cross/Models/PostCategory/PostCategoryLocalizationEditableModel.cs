using Newtonsoft.Json;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class PostCategoryLocalizationEditableModel
    {
        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }
    }
}
