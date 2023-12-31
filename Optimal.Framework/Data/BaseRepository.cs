﻿using Microsoft.EntityFrameworkCore;
using Optimal.Framework.Data.DataProvider;

namespace Optimal.Framework.Data
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IAppDataProvider _dataProvider;

        public IQueryable<TEntity> Table => _dataProvider.GetTable<TEntity>();

        public BaseRepository(IAppDataProvider dataProvider)
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

        public async Task InsertAsync(TEntity entity)
        {
            await _dataProvider.InsertEntity(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await _dataProvider.UpdateEntity(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            await _dataProvider.DeleteEntity(entity);
        }
    }
}
