using Microsoft.EntityFrameworkCore;
using Optimal.Framework.Data.DataProvider;

namespace Optimal.Framework.Data
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IDataProvider _dataProvider;

        public IQueryable<TEntity> Table => _dataProvider.GetTable<TEntity>();

        public BaseRepository(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Table.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
