using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.DataProvider.PostgreSQL;
using System.Data.Common;

namespace Optimal.Framework.Data.DataProvider.LinqToDB
{
    public class LinqToDBPostgreSQLDataProvider : PostgreSQLDataProvider
    {
        public LinqToDBPostgreSQLDataProvider() : base("PostgreSQL", PostgreSQLVersion.v95)
        {

        }

        public override void SetParameter(DataConnection dataConnection, DbParameter parameter, string name, DbDataType dataType, object value)
        {
            if (value is string && dataType.DataType == DataType.NVarChar)
            {
                dataType = dataType.WithDbType("citext");
            }
            base.SetParameter(dataConnection, parameter, name, dataType, value);
        }
    }

}
