using System.Collections.Generic;
using System.Linq;
using TFW.Docs.Cross.Models.PostCategory;

namespace TFW.Docs.Business.Core.Queries.PostCategory
{
    public static class ListPostCategoryJoinModelNamedQuery
    {
        public static IQueryable<ListPostCategoryJoinModel> BySearchTerm(this IQueryable<ListPostCategoryJoinModel> query, string searchTerm)
        {
            return query.Where(o => o.Title.Contains(searchTerm));
        }

        public static IQueryable<ListPostCategoryJoinModel> ByCulture(this IQueryable<ListPostCategoryJoinModel> query, string lang, string region)
        {
            if (!string.IsNullOrEmpty(region))
                return query.Where(o => o.Lang == lang && o.Region == region);

            return query.Where(o => o.Lang == lang);
        }

        public static IQueryable<ListPostCategoryJoinModel> ByCulture(this IQueryable<ListPostCategoryJoinModel> query, string culture)
        {
            return query.Where(o => (string.IsNullOrEmpty(o.Region) ? o.Lang : o.Lang + "-" + o.Region) == culture);
        }

        public static IQueryable<ListPostCategoryJoinModel> ByCultures(this IQueryable<ListPostCategoryJoinModel> query, IEnumerable<string> cultures)
        {
            return query.Where(o => cultures.Contains(string.IsNullOrEmpty(o.Region) ? o.Lang : o.Lang + "-" + o.Region));
        }
    }
}
