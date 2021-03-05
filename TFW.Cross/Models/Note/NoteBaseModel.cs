using Newtonsoft.Json;

namespace TFW.Cross.Models.Note
{
    public class NoteBaseModel : NoteSimpleModel
    {
        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }
    }
}
