using Newtonsoft.Json;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class CreatePostCategoryLocalizationModel : PostCategoryLocalizationEditableModel, ILocalizationModel
    {
        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }
    }
}
