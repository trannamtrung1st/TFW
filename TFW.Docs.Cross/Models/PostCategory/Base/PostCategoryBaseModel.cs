using Newtonsoft.Json;
using System;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class PostCategoryBaseModel : PostCategorySimpleModel
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; }
    }
}
