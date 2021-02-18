using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross.Models.Common;

namespace TFW.Data.Repositories
{
    public partial interface IBaseRepository<E> where E : class
    {
        IQueryable<T> Limit<T>(IQueryable<T> query, PagingQueryModel pagingModel);
        Task<EntityEntry<E>> ReloadAsync(E entity);
        EntityEntry<E> Add(E entity);
        void AddRange(IEnumerable<E> entities);
        EntityEntry<E> Update(E entity);
        void UpdateRange(IEnumerable<E> entities);
        EntityEntry<E> Update(E entity, Action<E> patchAction);
        EntityEntry<E> Update(E entity, params Expression<Func<E, object>>[] changedProperties);
        EntityEntry<E> Attach(E entity);
        bool TryAttach(E entity, out EntityEntry<E> entry);
        EntityEntry<E> Remove(E entity);
        Task<EntityEntry<E>> RemoveAsync(params object[] key);
        void RemoveRange(IEnumerable<E> list);
        Task<int> SqlRemoveAllAsync();
        Task<E> FindAsync(params object[] key);
        IQueryable<E> AsTracking();
        IQueryable<E> AsNoTracking();
        IQueryable<E> Get();
        Task<E> FirstOrDefaultAsync();
        string EntityName { get; }
    }
}
