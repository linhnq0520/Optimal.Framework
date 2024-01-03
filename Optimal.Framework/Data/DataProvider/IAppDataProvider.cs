namespace Optimal.Framework.Data.DataProvider
{
    public interface IAppDataProvider
    {
        IQueryable<TEntity> GetTable<TEntity>() where TEntity : BaseEntity;

        Task<TEntity> InsertEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;

        Task UpdateEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;

        Task DeleteEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;
    }
}
