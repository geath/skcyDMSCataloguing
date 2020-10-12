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
        IEnumerable<TEntity> GetAll();


        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, 
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeproperty = null,
                                                bool disableTracking = true);

        Task<TEntity> GetByConditionAsync(Expression<Func<TEntity, bool>> filter = null,
                                                        string includeProperties = "");

       
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task SaveAsync();        
    }
}