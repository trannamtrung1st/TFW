using System.Collections.Generic;
using TFW.Docs.Cross.Models.Common;
using AU = TFW.Docs.Cross.Entities.AppUser;

namespace TFW.Docs.Cross.Models.AppUser
{
    public class DynamicQueryAppUserModel : BaseDynamicQueryModel
    {
        protected override string[] DefaultFields { get; } = new[] { FieldInfo };

        #region Filter options
        public int? Id { get; set; }
        public string UserName { get; set; }
        public string SearchTerm { get; set; }
        #endregion

        #region Sorting constants
        public const string SortByUsername = "username";
        public const string DefaultSortBy = "a" + SortByUsername;

        public static readonly IEnumerable<string> SortOptions = new[] { SortByUsername };
        #endregion

        #region Projection constants
        public const string FieldInfo = "info";

        public static readonly IReadOnlyDictionary<string, string> Projections =
            new Dictionary<string, string>()
            {
                {
                    FieldInfo, $"{nameof(AU.Id)},{nameof(AU.UserName)},{nameof(AU.Email)}," +
                    $"{nameof(AU.FullName)},{nameof(AU.CreatedTime)}"
                },
            };
        #endregion
    }
}
