using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstracts.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

        TEntity Attach(TEntity entity);

        TEntity Find(params object[] keyValues);

        TEntity RemovePhysical(TEntity entity);

        T Remove<T>(T entity) where T : class, TEntity, ISoftDeleteEntity;

        IQueryable<TEntity> Get();

        IQueryable<TEntity> GetAsNoTracking();

        IQueryable<TEntity> IgnoreQueryFilters(IQueryable<TEntity> query);

        IQueryable<TEntity> IgnoreQueryFilters();
    }
}
