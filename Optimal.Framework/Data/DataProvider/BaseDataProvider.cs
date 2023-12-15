using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.SqlServer;
using LinqToDB.Linq;
using Optimal.Framework.ConfigManager.DataConfig;
using Optimal.Framework.Infrastructure;
namespace Optimal.Framework.Data.DataProvider
{
    public class BaseDataProvider : IAppDataProvider
    {
        protected LinqToDB.DataProvider.IDataProvider LinqToDbDataProvider => SqlServerTools.GetDataProvider(SqlServerVersion.v2012, SqlServerProvider.MicrosoftDataSqlClient);
        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : BaseEntity
        {
            var connectionString = Singleton<DataConfig>.Instance.ConnectionString;
            var context = new DataContext(LinqToDbDataProvider, connectionString) 
            { 
            }.GetTable<TEntity>();
            return context;
        }
    }
}
