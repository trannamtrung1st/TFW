using System;
using System.Collections.Generic;
using System.Linq;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Business.Core.Queries.PostCategory
{
    public static class PostCategoryNamedQuery
    {
        public static IQueryable<PostCategoryEntity> ById(this IQueryable<PostCategoryEntity> query, int id)
        {
            return query.Where(o => o.Id == id);
        }

        public static IQueryable<PostCategoryEntity> ByIds(this IQueryable<PostCategoryEntity> query, IEnumerable<int> ids)
        {
            return query.Where(o => ids.Contains(o.Id));
        }
    }
}
