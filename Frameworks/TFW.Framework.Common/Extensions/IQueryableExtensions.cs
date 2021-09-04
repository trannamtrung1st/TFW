using System;
using System.Linq;
using System.Linq.Expressions;

namespace TFW.Framework.Common.Extensions
{
    public static class IQueryableExtensions
    {
        public static bool IsOrdered<T>(this IQueryable<T> query)
        {
            return query.Expression.Type == typeof(IOrderedQueryable<T>);
        }

        public static IQueryable<T> SequentialOrderBy<T, TKey>(this IQueryable<T> query,
            Expression<Func<T, TKey>> keySelector)
        {
            if (query.IsOrdered())
                query = (query as IOrderedQueryable<T>).ThenBy(keySelector);
            else
                query = query.OrderBy(keySelector);

            return query;
        }

        public static IQueryable<T> SequentialOrderByDesc<T, TKey>(this IQueryable<T> query,
            Expression<Func<T, TKey>> keySelector)
        {
            if (query.IsOrdered())
                query = (query as IOrderedQueryable<T>).ThenByDescending(keySelector);
            else
                query = query.OrderByDescending(keySelector);
            return query;
        }
    }
}
