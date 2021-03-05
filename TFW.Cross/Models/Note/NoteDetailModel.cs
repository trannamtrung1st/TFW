using Newtonsoft.Json;
using TFW.Cross.Models.AppUser;
using TFW.Cross.Models.NoteCategory;

namespace TFW.Cross.Models.Note
{
    public class NoteDetailModel : NoteBaseModel
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("category")]
        public NoteCategorySimpleModel Category { get; set; }

        [JsonProperty("createdUser")]
        public AppUserSimpleModel CreatedUser { get; set; }
    }
}
