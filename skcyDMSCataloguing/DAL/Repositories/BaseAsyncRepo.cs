using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.DAL.Repositories
{
    public class BaseAsyncRepo<TEntity> :IDisposable, IBaseAsyncRepo<TEntity> where TEntity : class
    {
        private AppDbContext _context;
        private DbSet<TEntity> _dbSet;

        public BaseAsyncRepo(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> filter = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;

            if (disableTracking)
            { query = query.AsNoTracking(); }

            if (filter != null)
            { query = query.Where(filter).AsNoTracking(); }


            return query.ToList();
        }



        public async Task<IEnumerable<TEntity>> GetAllAsync(
                                                            Expression<Func<TEntity, bool>> filter = null,
                                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeproperty = null,
                                                            bool disableTracking = true
                                                            )
        {
            IQueryable<TEntity> query = _dbSet;

            if (disableTracking)
            { query = query.AsNoTracking(); }

            if (includeproperty != null)
            { query = includeproperty(query); }

            if (filter != null)
            { query = query.Where(filter).AsNoTracking(); }

            if (orderBy != null)
            { return await orderBy(query).ToListAsync(); }
            else
            { return await query.ToListAsync(); }
        }

        public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync();
        }


        public async Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>> filter = null,
                                                string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            query = query.Where(filter).AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }


        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        // dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
