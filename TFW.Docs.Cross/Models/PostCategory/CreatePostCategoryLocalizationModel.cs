using Newtonsoft.Json;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class CreatePostCategoryLocalizationModel : PostCategoryLocalizationEditableModel, ICreateLocalizationModel
    {
        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }
    }
}
