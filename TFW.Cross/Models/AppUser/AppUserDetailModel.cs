using Newtonsoft.Json;
using System.Collections.Generic;
using TFW.Cross.Models.Note;

namespace TFW.Cross.Models.AppUser
{
    public class AppUserDetailModel : AppUserBaseModel
    {
        [JsonProperty("notes")]
        public IEnumerable<NoteBaseModel> Notes { get; set; }
    }
}
