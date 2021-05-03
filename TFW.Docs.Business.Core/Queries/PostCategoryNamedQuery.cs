using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Business.Core.Queries
{
    public static class PostCategoryNamedQuery
    {
        public static IQueryable<PostCategoryEntity> ById(this IQueryable<PostCategoryEntity> query, int id)
        {
            return query.Where(o => o.Id == id);
        }
    }
}
