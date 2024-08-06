using System.Reflection;

namespace Optimal.Framework.Data.Migration
{
    public interface IMigrationManager
    {
        void ApplyUpMigrations(Assembly assembly, MigrationProcessType migrationProcessType = MigrationProcessType.Installation);
        void ApplyDownMigrations(Assembly assembly);
    }
}