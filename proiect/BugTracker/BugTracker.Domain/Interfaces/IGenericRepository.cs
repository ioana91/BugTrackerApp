using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> GetByIdAsync(string id);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby, string includeProperties);
        //Task<ICollection<TEntity>> GetAllAsync();

        void Insert(TEntity entity);

        void Update(TEntity entity);

        void Delete(int id);
        void Delete(TEntity entity);
    }
}
