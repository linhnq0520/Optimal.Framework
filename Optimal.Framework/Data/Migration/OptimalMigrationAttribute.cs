using System.Globalization;
using FluentMigrator;

namespace Optimal.Framework.Data.Migration
{
	public class OptimalMigrationAttribute : MigrationAttribute
	{
		public MigrationProcessType TargetMigrationProcess { get; set; }

		private static long GetVersion(string dateTime)
		{
			return DateTime.ParseExact(dateTime, OptimalMigrationDefaults.DateFormats, CultureInfo.InvariantCulture).Ticks;
		}

		private static long GetVersion(string dateTime, UpdateMigrationType migrationType)
		{
			return GetVersion(dateTime) + (long)migrationType;
		}

		private static string GetDescription(string neptuneVersion, UpdateMigrationType migrationType)
		{
			return string.Format(OptimalMigrationDefaults.UpdateMigrationDescription, neptuneVersion, migrationType.ToString());
		}

		public OptimalMigrationAttribute(string dateTime, MigrationProcessType targetMigrationProcess = MigrationProcessType.NoMatter)
			: base(GetVersion(dateTime), null)
		{
			TargetMigrationProcess = targetMigrationProcess;
		}

		public OptimalMigrationAttribute(string dateTime, string description, MigrationProcessType targetMigrationProcess = MigrationProcessType.NoMatter)
			: base(GetVersion(dateTime), description)
		{
			TargetMigrationProcess = targetMigrationProcess;
		}

		public OptimalMigrationAttribute(string dateTime, string neptuneVersion, UpdateMigrationType migrationType, MigrationProcessType targetMigrationProcess = MigrationProcessType.NoMatter)
			: base(GetVersion(dateTime, migrationType), GetDescription(neptuneVersion, migrationType))
		{
			TargetMigrationProcess = targetMigrationProcess;
		}
	}
}
