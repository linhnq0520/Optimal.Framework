using System.Reflection;

namespace Optimal.Framework.Infrastructure
{
    public interface ITypeFinder
    {
        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        IList<Assembly> GetAssemblies();
    }

}
