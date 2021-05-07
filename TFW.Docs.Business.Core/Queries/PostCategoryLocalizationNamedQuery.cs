using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Business.Core.Queries
{
    public static class PostCategoryLocalizationNamedQuery
    {
        public static IQueryable<PostCategoryLocalizationEntity> ByCulture(this IQueryable<PostCategoryLocalizationEntity> query, string lang, string region)
        {
            return query.Where(o => o.Lang == lang && o.Region == region);
        }

        public static IEnumerable<PostCategoryLocalizationEntity> ByCulture(this IEnumerable<PostCategoryLocalizationEntity> query, string lang, string region)
        {
            return query.Where(o => o.Lang == lang && o.Region == region);
        }
    }
}
