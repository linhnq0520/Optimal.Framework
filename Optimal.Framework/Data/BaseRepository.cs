using Microsoft.EntityFrameworkCore;

namespace Optimal.Framework.Data
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public IQueryable<TEntity> Table => _context.Set<TEntity>();

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Table.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
            await SaveAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;
            entry.Property(x => x.Id).IsModified = false;

            await SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            TEntity entity = await Table.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                _context.Remove(entity);
                await SaveAsync();
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
