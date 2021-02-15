using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Cross.Entities;
using TFW.Framework.Common;

namespace TFW.Cross.Models
{
    public class GetAppUserListRequestModel : BaseGetListRequestModel
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string search { get; set; }
    }

    public class DynamicQueryAppUserModel : BaseDynamicQueryModel
    {
        public DynamicQueryAppUserModel()
        {
            defaultField = FieldInfo;
        }

        // filter
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Search { get; set; }

        //---------------------------------------
        public const string SortByUsername = "username";
        public const string DefaultSortBy = "a" + SortByUsername;

        public const string FieldInfo = "info";
        public const string FieldNotes = "notes";

        public static readonly IDictionary<string, string> Projections =
            new Dictionary<string, string>()
            {
                {
                    FieldInfo, $"{nameof(AppUser.Id)},{nameof(AppUser.UserName)},{nameof(AppUser.Email)}," +
                    $"{nameof(AppUser.FullName)}"
                },
                {
                    FieldNotes, $"{nameof(AppUser.Notes)}" +
                    $".Select(new {nameof(Note)}({nameof(Note.Title)},{nameof(Note.CategoryName)})).ToList() as {nameof(AppUser.Notes)}"
                }
            };
    }

    public class AppUserResponseModel
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
    }
}
