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

        TEntity Remove(TEntity entity);

        IQueryable<TEntity> Get();

        IQueryable<TEntity> GetAsNoTracking();
    }
}
