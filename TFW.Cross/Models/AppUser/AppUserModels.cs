using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using TFW.Cross.Models.Common;
using AU = TFW.Cross.Entities.AppUser;
using N = TFW.Cross.Entities.Note;

namespace TFW.Cross.Models.AppUser
{
    public class GetAppUserListRequestModel : BaseGetListRequestModel
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string searchTerm { get; set; }
    }

    public class DynamicQueryAppUserModel : BaseDynamicQueryModel
    {
        protected override string[] DefaultFields { get; } = new[] { FieldInfo };

        #region Filter options
        public string Id { get; set; }
        public string UserName { get; set; }
        public string SearchTerm { get; set; }
        #endregion

        #region Sorting constants
        public const string SortByUsername = "username";
        public const string DefaultSortBy = "a" + SortByUsername;

        public static readonly IEnumerable<string> SortOptions = ImmutableArray.Create(SortByUsername);
        #endregion

        #region Projection constants
        public const string FieldInfo = "info";
        public const string FieldNotes = "notes";

        public static readonly IReadOnlyDictionary<string, string> Projections =
            new Dictionary<string, string>()
            {
                {
                    FieldInfo, $"{nameof(AU.Id)},{nameof(AU.UserName)},{nameof(AU.Email)}," +
                    $"{nameof(AU.FullName)}"
                },
                {
                    FieldNotes, $"{nameof(AU.Notes)}" +
                    $".Select(new {typeof(NoteResponseModel).FullName}" +
                    $"({nameof(N.Title)},{nameof(N.CategoryName)})).ToList() as {nameof(AU.Notes)}"
                }
            }.ToImmutableDictionary();
        #endregion
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

        [JsonProperty("notes")]
        public IEnumerable<NoteResponseModel> Notes { get; set; }
    }

    public class NoteResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }
    }
}
