using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Business.Core.Queries
{
    public static class PostCategoryLocalizationNamedQuery
    {
        public static IQueryable<PostCategoryLocalizationEntity> ByIds(this IQueryable<PostCategoryLocalizationEntity> query, IEnumerable<int> ids)
        {
            return query.Where(o => ids.Contains(o.Id));
        }

        public static IQueryable<PostCategoryLocalizationEntity> ByCulture(this IQueryable<PostCategoryLocalizationEntity> query, string lang, string region)
        {
            return query.Where(o => o.Lang == lang && o.Region == region);
        }

        public static IQueryable<PostCategoryLocalizationEntity> ByCulture(this IQueryable<PostCategoryLocalizationEntity> query, string culture)
        {
            return query.Where(o => (string.IsNullOrEmpty(o.Region) ? o.Lang : (o.Lang + "-" + o.Region)) == culture);
        }

        public static IQueryable<PostCategoryLocalizationEntity> ByCultures(this IQueryable<PostCategoryLocalizationEntity> query, IEnumerable<string> cultures)
        {
            return query.Where(o => cultures.Contains((string.IsNullOrEmpty(o.Region) ? o.Lang : (o.Lang + "-" + o.Region))));
        }
    }
}
