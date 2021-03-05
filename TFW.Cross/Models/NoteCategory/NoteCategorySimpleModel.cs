using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Models.NoteCategory
{
    public class NoteCategorySimpleModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
