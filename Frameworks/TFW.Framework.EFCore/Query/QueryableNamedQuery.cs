using System;
using System.Linq;
using TFW.Framework.Cross.Audit;

namespace TFW.Framework.EFCore.Query
{
    public static class QueryableNamedQuery
    {
        public static IQueryable<T> Limit<T>(this IQueryable<T> query, int page, int pageLimit)
        {
            if (page <= 0 || pageLimit <= 0)
                throw new InvalidOperationException("Invalid paging request");

            query = query.Skip((page - 1) * pageLimit).Take(pageLimit);

            return query;
        }

        public static IQueryable<T> IsDeleted<T>(this IQueryable<T> query, bool value = true) where T : ISoftDeleteEntity
        {
            return query.Where(o => o.IsDeleted == value);
        }
    }
}
