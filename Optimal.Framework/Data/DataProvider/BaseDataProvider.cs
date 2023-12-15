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
using LinqToDB.Linq;
using Optimal.Framework.ConfigManager.DataConfig;
using Optimal.Framework.Infrastructure;
namespace Optimal.Framework.Data.DataProvider
{
    public class BaseDataProvider : IDataProvider
    {
        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : BaseEntity
        {
            var connectionString = Singleton<DataConfig>.Instance.ConnectionString;
            return new DataContext(connectionString).GetTable<TEntity>();
        }
    }
}
