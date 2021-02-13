using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TFW.Framework.Common
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> SequentialOrderBy<T, TKey>(this IQueryable<T> query,
            Expression<Func<T, TKey>> keySelector)
        {
            if (query is IOrderedQueryable<T>)
                query = (query as IOrderedQueryable<T>).ThenBy(keySelector);
            else
                query = query.OrderBy(keySelector);
            return query;
        }

        public static IQueryable<T> SequentialOrderByDesc<T, TKey>(this IQueryable<T> query,
            Expression<Func<T, TKey>> keySelector)
        {
            if (query is IOrderedQueryable<T>)
                query = (query as IOrderedQueryable<T>).ThenByDescending(keySelector);
            else
                query = query.OrderByDescending(keySelector);
            return query;
        }
    }
}
