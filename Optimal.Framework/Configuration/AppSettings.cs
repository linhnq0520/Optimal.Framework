using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace Optimal.Framework.Configuration
{
    public class AppSettings(IList<IConfig> configurations = null)
    {
        private readonly Dictionary<Type, IConfig> _configurations =
            configurations
                ?.OrderBy((IConfig config) => config.GetOrder())
                ?.ToDictionary((IConfig config) => config.GetType(), (IConfig config) => config)
            ?? [];

        [JsonExtensionData]
        public Dictionary<string, JToken> Configuration { get; set; } = [];

        public TConfig Get<TConfig>()
            where TConfig : class, IConfig
        {
            //if (_configurations[typeof(TConfig)] is null || _configurations[typeof(TConfig)] is not TConfig result)
            //{
            //    throw new Exception($"No configuration with type '{typeof(TConfig)}' found");
            //}
            var result = Configuration[typeof(TConfig).Name].ToObject<TConfig>();

            return result;
        }

        public void Update(IList<IConfig> configurations)
        {
            foreach (IConfig configuration in configurations)
            {
                _configurations[configuration.GetType()] = configuration;
            }
        }
    }
}
