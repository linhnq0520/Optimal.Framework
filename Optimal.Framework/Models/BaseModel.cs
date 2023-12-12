using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Optimal.Framework.Models
{

    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract class BaseModel
    {
    }
}
