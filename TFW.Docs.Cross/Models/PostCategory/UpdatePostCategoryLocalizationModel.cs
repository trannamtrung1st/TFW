using Newtonsoft.Json;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class UpdatePostCategoryLocalizationModel : PostCategoryLocalizationEditableModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
