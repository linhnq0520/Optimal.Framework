using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.SqlServer;
using Optimal.Framework.ConfigManager.DataConfig;
using Optimal.Framework.Infrastructure;

namespace Optimal.Framework.Data.DataProvider
{
    public class BaseDataProvider : IAppDataProvider
    {
        protected readonly string _connectionString = Singleton<DataConfig>.Instance.ConnectionString;
        protected IDataProvider LinqToDbDataProvider => SqlServerTools.GetDataProvider(SqlServerVersion.v2012, SqlServerProvider.MicrosoftDataSqlClient);

        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : BaseEntity
        {
            var context = new DataContext(LinqToDbDataProvider, _connectionString) 
            { 
            }.GetTable<TEntity>();
            return context;
        }

        protected virtual DataConnection CreateDataConnection()
        {
            return CreateDataConnection(LinqToDbDataProvider);
        }

        protected virtual DataConnection CreateDataConnection(IDataProvider dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            return new DataConnection(LinqToDbDataProvider, _connectionString);
        }

        public async Task<TEntity> InsertEntity<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            using DataConnection dataContext = CreateDataConnection();
            entity.Id = await dataContext.InsertWithInt32IdentityAsync(entity);
            return entity;
        }

        public async Task UpdateEntity<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            using DataConnection dataContext = CreateDataConnection();
            await dataContext.UpdateAsync(entity);
        }

        public async Task DeleteEntity<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            using DataConnection dataContext = CreateDataConnection();
            await dataContext.DeleteAsync(entity);
        }
    }
}
