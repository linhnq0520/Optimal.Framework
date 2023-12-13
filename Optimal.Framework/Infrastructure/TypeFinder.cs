using System.Reflection;

namespace Optimal.Framework.Infrastructure
{
    public class TypeFinder : ITypeFinder
    {
        public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
        {
            Type assignTypeFrom = typeof(T);
            List<Type> result = new List<Type>();

            foreach (Assembly assembly in GetAssemblies())
            {
                try
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (assignTypeFrom.IsAssignableFrom(type) && !type.IsInterface && (!onlyConcreteClasses || (type.IsClass && !type.IsAbstract)))
                        {
                            result.Add(type);
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    HandleReflectionTypeLoadException(ex);
                }
            }

            return result;
        }

        public virtual IList<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().ToList();
        }

        protected virtual void HandleReflectionTypeLoadException(ReflectionTypeLoadException ex)
        {
            foreach (Exception loaderException in ex.LoaderExceptions)
            {
                Console.WriteLine($"ReflectionTypeLoadException: {loaderException.Message}");
            }
        }
    }
}
