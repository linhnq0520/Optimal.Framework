namespace Optimal.Framework.Data.ConfigManager
{
    public class DataConfig
    {
        public string ConnectionString { get; set; }

        public string DataProvider { get; set; }

        public int? SQLCommandTimeout { get; set; }
    }
}
