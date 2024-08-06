using System.Text.Json.Serialization;

namespace Optimal.Framework.Configuration
{
    public interface IConfig
    {
        [JsonIgnore]
        string Name => GetType().Name;

        int GetOrder()
        {
            return 1;
        }
    }
}
