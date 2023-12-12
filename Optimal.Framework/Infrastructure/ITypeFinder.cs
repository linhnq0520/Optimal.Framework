using System.Reflection;

namespace Optimal.Framework.Infrastructure
{
    public interface ITypeFinder
    {
        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        IList<Assembly> GetAssemblies();
    }

}
