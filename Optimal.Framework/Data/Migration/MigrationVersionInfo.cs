using FluentMigrator.Runner.VersionTableInfo;

namespace Optimal.Framework.Data.Migration
{
    public class MigrationVersionInfo : BaseEntity, IVersionTableMetaData
    {
        public long Version { get; set; }

        public string Description { get; set; }

        public DateTime AppliedOn { get; set; }

        public object ApplicationContext { get; set; }

        public bool OwnsSchema { get; } = true;


        public string SchemaName { get; } = string.Empty;


        public string TableName { get; }

        public string ColumnName { get; }

        public string DescriptionColumnName { get; }

        public string UniqueIndexName { get; } = "UC_Version";


        public string AppliedOnColumnName { get; }

        public MigrationVersionInfo()
        {
            TableName = "MigrationVersionInfo";
            ColumnName = "Version";
            DescriptionColumnName = "Description";
            AppliedOnColumnName = "AppliedOn";
        }
    }
}