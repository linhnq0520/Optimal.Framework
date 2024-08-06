using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace Optimal.Framework.Configuration
{
    public class AppSettings
    {
        private readonly Dictionary<Type, IConfig> _configurations = [];

        [JsonExtensionData]
        public Dictionary<string, JToken> Configuration { get; set; }

        public AppSettings(IList<IConfig> configurations = null)
        {
            _configurations = configurations?.OrderBy((IConfig config) => config.GetOrder())?.ToDictionary((IConfig config) => config.GetType(), (IConfig config) => config) ?? new Dictionary<Type, IConfig>();
        }

        public TConfig Get<TConfig>() where TConfig : class, IConfig
        {
            if (_configurations[typeof(TConfig)] is not TConfig result)
            {
                throw new Exception($"No configuration with type '{typeof(TConfig)}' found");
            }

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
