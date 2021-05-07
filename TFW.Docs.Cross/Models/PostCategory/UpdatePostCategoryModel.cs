﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class UpdatePostCategoryModel : PostCategoryEditableModel
    {
        [JsonProperty("startingPostId")]
        public int? StartingPostId { get; set; }
    }
}