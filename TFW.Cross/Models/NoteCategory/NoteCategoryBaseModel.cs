using Newtonsoft.Json;

namespace TFW.Cross.Models.NoteCategory
{
    public class NoteCategoryBaseModel : NoteCategorySimpleModel
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
