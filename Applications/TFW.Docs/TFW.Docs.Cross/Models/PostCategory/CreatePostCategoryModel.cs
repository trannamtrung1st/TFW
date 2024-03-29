﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class CreatePostCategoryModel : PostCategoryEditableModel, ICreateLocalizedModel<CreatePostCategoryLocalizationModel>
    {
        [JsonProperty("listOfLocalization")]
        public IEnumerable<CreatePostCategoryLocalizationModel> ListOfLocalization { get; set; }
    }
}
