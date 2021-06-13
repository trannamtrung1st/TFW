using Application.Abstracts.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DataContext _dbContext;

        public Repository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TEntity Add(TEntity entity)
        {
            return _dbContext.Add(entity).Entity;
        }

        public TEntity Attach(TEntity entity)
        {
            return _dbContext.Attach(entity).Entity;
        }

        public TEntity Find(params object[] keyValues)
        {
            return _dbContext.Find<TEntity>(keyValues);
        }

        public IQueryable<TEntity> Get()
        {
            return _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAsNoTracking()
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }

        public TEntity Remove(TEntity entity)
        {
            return _dbContext.Remove(entity).Entity;
        }

        public TEntity Update(TEntity entity)
        {
            return _dbContext.Update(entity).Entity;
        }
    }
}
