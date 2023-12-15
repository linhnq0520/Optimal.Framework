using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimal.Framework.Data
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> Table { get; }
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        //Task AddAsync(TEntity entity);
        //Task UpdateAsync(TEntity entity);
        //Task DeleteAsync(int id);
        //Task SaveAsync();
    }
}
