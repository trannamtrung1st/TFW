using Newtonsoft.Json;
using System.Collections.Generic;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class PostCategoryDetailModel : PostCategoryBaseModel
    {
        [JsonProperty("startingPostId")]
        public int? StartingPostId { get; set; }
    }
}
