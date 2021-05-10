using Newtonsoft.Json;
using System.Collections.Generic;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class UpdatePostCategoryLocalizationsModel
    {
        [JsonProperty("listOfLocalization")]
        public IEnumerable<UpdatePostCategoryLocalizationModel> ListOfLocalization { get; set; }
    }
}
