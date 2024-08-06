namespace Optimal.Framework.Data.Migration
{
    public class OptimalMigrationDefaults
    {
        public static string[] DateFormats { get; } = ["yyyy-MM-dd HH:mm:ss", "yyyy.MM.dd HH:mm:ss", "yyyy/MM/dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss:fffffff", "yyyy.MM.dd HH:mm:ss:fffffff", "yyyy/MM/dd HH:mm:ss:fffffff"];
        public static string UpdateMigrationDescription { get; } = "Neptune version {0}. Update {1}";
        public static string UpdateMigrationDescriptionPrefix { get; } = "Neptune version {0}. Update";
    }
}