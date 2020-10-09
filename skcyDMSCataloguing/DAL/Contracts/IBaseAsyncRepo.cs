using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.DAL.Repositories
{
    public interface IBaseAsyncRepo<TEntity> where TEntity : class
    {
        void Delete(TEntity entity);
        IEnumerable<TEntity> Get();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeproperty = null, bool disableTracking = true);
        IEnumerable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> filter = null, bool disableTracking = true);
        Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");
        void Insert(TEntity entity);
        Task SaveAsync();
        void Update(TEntity entity);
    }
}