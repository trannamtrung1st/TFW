using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class PostCategorySimpleModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
