using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.Common.Query;

namespace TFW.Framework.EFCore.Query
{
    public class EFCoreAsyncQueryExecutor : IAsyncQueryExecutor
    {
        public Task<bool> AllAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.AllAsync(predicate, cancellationToken);
        }

        public Task<bool> AnyAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.AnyAsync(predicate, cancellationToken);
        }

        public Task<bool> AnyAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.AnyAsync(cancellationToken);
        }

        public IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(IQueryable<TSource> source)
        {
            return source.AsAsyncEnumerable();
        }

        public Task<decimal> AverageAsync(IQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(cancellationToken);
        }

        public Task<decimal> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(selector, cancellationToken);
        }

        public Task<decimal?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(selector, cancellationToken);
        }

        public Task<double> AverageAsync(IQueryable<int> source, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(cancellationToken);
        }

        public Task<double?> AverageAsync(IQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(cancellationToken);
        }

        public Task<double> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(selector, cancellationToken);
        }

        public Task<double?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(selector, cancellationToken);
        }

        public Task<double> AverageAsync(IQueryable<long> source, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(cancellationToken);
        }

        public Task<double?> AverageAsync(IQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(cancellationToken);
        }

        public Task<double> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(selector, cancellationToken);
        }

        public Task<double> AverageAsync(IQueryable<double> source, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(cancellationToken);
        }

        public Task<decimal?> AverageAsync(IQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(cancellationToken);
        }

        public Task<double?> AverageAsync(IQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(cancellationToken);
        }

        public Task<double> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(selector, cancellationToken);
        }

        public Task<double?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(selector, cancellationToken);
        }

        public Task<float> AverageAsync(IQueryable<float> source, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(cancellationToken);
        }

        public Task<float?> AverageAsync(IQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(cancellationToken);
        }

        public Task<float> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(selector, cancellationToken);
        }

        public Task<float?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(selector, cancellationToken);
        }

        public Task<double?> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            return source.AverageAsync(selector, cancellationToken);
        }

        public Task<bool> ContainsAsync<TSource>(IQueryable<TSource> source, TSource item, CancellationToken cancellationToken = default)
        {
            return source.ContainsAsync(item, cancellationToken);
        }

        public Task<int> CountAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.CountAsync(cancellationToken);
        }

        public Task<int> CountAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.CountAsync(predicate, cancellationToken);
        }

        public Task<TSource> FirstAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.FirstAsync(predicate, cancellationToken);
        }

        public Task<TSource> FirstAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.FirstAsync(cancellationToken);
        }

        public Task<TSource> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.FirstOrDefaultAsync(cancellationToken);
        }

        public Task<TSource> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public Task ForEachAsync<T>(IQueryable<T> source, Action<T> action, CancellationToken cancellationToken = default)
        {
            return source.ForEachAsync(action, cancellationToken);
        }

        public Task<TSource> LastAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.LastAsync(cancellationToken);
        }

        public Task<TSource> LastAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.LastAsync(predicate, cancellationToken);
        }

        public Task<TSource> LastOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.LastOrDefaultAsync(cancellationToken);
        }

        public Task<TSource> LastOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.LastOrDefaultAsync(predicate, cancellationToken);
        }

        public Task<long> LongCountAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.LongCountAsync(predicate, cancellationToken);
        }

        public Task<long> LongCountAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.LongCountAsync(cancellationToken);
        }

        public Task<TSource> MaxAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.MaxAsync(cancellationToken);
        }

        public Task<TResult> MaxAsync<TSource, TResult>(IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default)
        {
            return source.MaxAsync(selector, cancellationToken);
        }

        public Task<TSource> MinAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.MinAsync(cancellationToken);
        }

        public Task<TResult> MinAsync<TSource, TResult>(IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default)
        {
            return source.MinAsync(selector, cancellationToken);
        }

        public Task<TSource> SingleAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.SingleAsync(cancellationToken);
        }

        public Task<TSource> SingleAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.SingleAsync(predicate, cancellationToken);
        }

        public Task<TSource> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public Task<TSource> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.SingleOrDefaultAsync(cancellationToken);
        }

        public Task<int> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(selector, cancellationToken);
        }

        public Task<int?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(selector, cancellationToken);
        }

        public Task<decimal?> SumAsync(IQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(cancellationToken);
        }

        public Task<long> SumAsync(IQueryable<long> source, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(cancellationToken);
        }

        public Task<long?> SumAsync(IQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(cancellationToken);
        }

        public Task<long> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(selector, cancellationToken);
        }

        public Task<long?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(selector, cancellationToken);
        }

        public Task<double> SumAsync(IQueryable<double> source, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(cancellationToken);
        }

        public Task<double?> SumAsync(IQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(cancellationToken);
        }

        public Task<decimal> SumAsync(IQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(cancellationToken);
        }

        public Task<double?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(selector, cancellationToken);
        }

        public Task<double> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(selector, cancellationToken);
        }

        public Task<float> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(selector, cancellationToken);
        }

        public Task<decimal> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(selector, cancellationToken);
        }

        public Task<decimal?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(selector, cancellationToken);
        }

        public Task<float> SumAsync(IQueryable<float> source, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(cancellationToken);
        }

        public Task<float?> SumAsync(IQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(cancellationToken);
        }

        public Task<float?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(selector, cancellationToken);
        }

        public Task<int?> SumAsync(IQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(cancellationToken);
        }

        public Task<int> SumAsync(IQueryable<int> source, CancellationToken cancellationToken = default)
        {
            return source.SumAsync(cancellationToken);
        }

        public Task<TSource[]> ToArrayAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.ToArrayAsync(cancellationToken);
        }

        public Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(IQueryable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken = default)
        {
            return source.ToDictionaryAsync(keySelector, cancellationToken);
        }

        public Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(IQueryable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            return source.ToDictionaryAsync(keySelector, comparer, cancellationToken);
        }

        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(IQueryable<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken = default)
        {
            return source.ToDictionaryAsync(keySelector, elementSelector, cancellationToken);
        }

        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(IQueryable<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            return source.ToDictionaryAsync(keySelector, elementSelector, comparer, cancellationToken);
        }

        public Task<List<TSource>> ToListAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.ToListAsync(cancellationToken);
        }
    }
}
