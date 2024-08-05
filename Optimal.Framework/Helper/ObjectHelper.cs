using Newtonsoft.Json;

namespace Optimal.Framework.Helper
{
    public static partial class ObjectHelper
    {
        public static T ToObject<T>(this object @object)
        {
            if (@object == null) return default;

            if (@object is string stringObject)
            {
                return JsonConvert.DeserializeObject<T>(stringObject);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(@object));
            }
        }
    }
}
