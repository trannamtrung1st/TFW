using Newtonsoft.Json;
using System;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class PostCategoryLocalizationDetailModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("lastModifiedTime")]
        public DateTime? LastModifiedTime { get; set; }

        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; }
    }
}
