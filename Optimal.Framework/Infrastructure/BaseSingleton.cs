
namespace Optimal.Framework.Infrastructure
{
    public class BaseSingleton
    {
        public static IDictionary<Type, object> AllSingletons { get; }

        static BaseSingleton()
        {
            AllSingletons = new Dictionary<Type, object>();
        }
    }

    public class Singleton<T> : BaseSingleton
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
                BaseSingleton.AllSingletons[typeof(T)] = value;
            }
        }
    }

    public class SingletonDictionary<TKey, TValue> : Singleton<IDictionary<TKey, TValue>>
    {
        public new static IDictionary<TKey, TValue> Instance => Singleton<Dictionary<TKey, TValue>>.Instance;

        static SingletonDictionary()
        {
            Singleton<Dictionary<TKey, TValue>>.Instance = new Dictionary<TKey, TValue>();
        }
    }
}
