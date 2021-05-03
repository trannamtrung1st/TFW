using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class CreatePostCategoryModel : PostCategoryEditableModel
    {
        [JsonProperty("listOfLocalization")]
        public IEnumerable<CreatePostCategoryLocalizationModel> ListOfLocalization { get; set; }
    }
}
