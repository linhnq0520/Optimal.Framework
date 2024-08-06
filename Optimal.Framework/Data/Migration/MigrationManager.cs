using System.Reflection;
using FluentMigrator;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Optimal.Framework.Infrastructure;

namespace Optimal.Framework.Data.Migration
{
    public class MigrationManager : IMigrationManager
    {
        private readonly IFilteringMigrationSource _filteringMigrationSource;

        private readonly IMigrationRunner _migrationRunner;

        private readonly IMigrationRunnerConventions _migrationRunnerConventions;

        private readonly Lazy<IVersionLoader> _versionLoader;

        public MigrationManager(IFilteringMigrationSource filteringMigrationSource, IMigrationRunner migrationRunner, IMigrationRunnerConventions migrationRunnerConventions)
        {
            _versionLoader = new Lazy<IVersionLoader>(() => EngineContext.Current.Resolve<IVersionLoader>());
            _filteringMigrationSource = filteringMigrationSource;
            _migrationRunner = migrationRunner;
            _migrationRunnerConventions = migrationRunnerConventions;
        }

        protected virtual IEnumerable<IMigrationInfo> GetUpMigrations(Assembly assembly, MigrationProcessType migrationProcessType = MigrationProcessType.NoMatter)
        {
            IEnumerable<IMigration> source = _filteringMigrationSource.GetMigrations(delegate (Type t)
            {
                OptimalMigrationAttribute customAttribute = t.GetCustomAttribute<OptimalMigrationAttribute>();
                if (customAttribute == null || _versionLoader.Value.VersionInfo.HasAppliedMigration(customAttribute.Version))
                {
                    return false;
                }
                return (customAttribute.TargetMigrationProcess == MigrationProcessType.NoMatter || migrationProcessType == MigrationProcessType.NoMatter || migrationProcessType == customAttribute.TargetMigrationProcess) && (assembly == null || t.Assembly == assembly);
            }) ?? Enumerable.Empty<IMigration>();
            return from m in source
                   select _migrationRunnerConventions.GetMigrationInfoForMigration(m) into migration
                   orderby migration.Version
                   select migration;
        }

        protected virtual IEnumerable<IMigrationInfo> GetDownMigrations(Assembly assembly, MigrationProcessType migrationProcessType = MigrationProcessType.NoMatter)
        {
            IEnumerable<IMigration> source = _filteringMigrationSource.GetMigrations(delegate (Type t)
            {
                OptimalMigrationAttribute customAttribute = t.GetCustomAttribute<OptimalMigrationAttribute>();
                if (customAttribute == null || !_versionLoader.Value.VersionInfo.HasAppliedMigration(customAttribute.Version))
                {
                    return false;
                }
                return (customAttribute.TargetMigrationProcess == MigrationProcessType.NoMatter || migrationProcessType == MigrationProcessType.NoMatter || migrationProcessType == customAttribute.TargetMigrationProcess) && (assembly == null || t.Assembly == assembly);
            }) ?? Enumerable.Empty<IMigration>();
            return from m in source
                   select _migrationRunnerConventions.GetMigrationInfoForMigration(m) into migration
                   orderby migration.Version
                   select migration;
        }

        public void ApplyUpMigrations(Assembly assembly, MigrationProcessType migrationProcessType = MigrationProcessType.Installation)
        {
            if ((object)assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            foreach (IMigrationInfo upMigration in GetUpMigrations(assembly, migrationProcessType))
            {
                _migrationRunner.Up(upMigration.Migration);
                if (string.IsNullOrEmpty(upMigration.Description) || !upMigration.Description.StartsWith(string.Format(OptimalMigrationDefaults.UpdateMigrationDescriptionPrefix, "1")))
                {
                    _versionLoader.Value.UpdateVersionInfo(upMigration.Version, upMigration.Description ?? upMigration.Migration.GetType().Name);
                }
            }
        }

        public void ApplyDownMigrations(Assembly assembly)
        {
            if ((object)assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            foreach (IMigrationInfo item in GetDownMigrations(assembly).Reverse())
            {
                _migrationRunner.Down(item.Migration);
                _versionLoader.Value.DeleteVersion(item.Version);
            }
        }
    }
}