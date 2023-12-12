using System.Reflection;

namespace Optimal.Framework.Infrastructure
{
    public class TypeFinder : ITypeFinder
    {
        protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            try
            {
                Type genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
                Type[] array = type.FindInterfaces((Type objType, object objCriteria) => true, null);
                foreach (Type implementedInterface in array)
                {
                    if (implementedInterface.IsGenericType && genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition()))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        protected virtual IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            List<Type> result = new List<Type>();
            try
            {
                foreach (Assembly a in assemblies)
                {
                    Type[] types = null;
                    try
                    {
                        types = a.GetTypes();
                    }
                    catch
                    {
                        throw;
                    }
                    if (types == null)
                    {
                        continue;
                    }
                    Type[] array = types;
                    foreach (Type t in array)
                    {
                        if ((!assignTypeFrom.IsAssignableFrom(t) && (!assignTypeFrom.IsGenericTypeDefinition || !DoesTypeImplementOpenGeneric(t, assignTypeFrom))) || t.IsInterface)
                        {
                            continue;
                        }
                        if (onlyConcreteClasses)
                        {
                            if (t.IsClass && !t.IsAbstract)
                            {
                                result.Add(t);
                            }
                        }
                        else
                        {
                            result.Add(t);
                        }
                    }
                }
                return result;
            }
            catch (ReflectionTypeLoadException ex)
            {
                string msg = string.Empty;
                Exception[] loaderExceptions = ex.LoaderExceptions;
                foreach (Exception e in loaderExceptions)
                {
                    msg = msg + e.Message + Environment.NewLine;
                }
                throw new Exception(msg, ex);
            }
        }
        public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true)
        {
            throw new NotImplementedException();
        }

        public IList<Assembly> GetAssemblies()
        {
            throw new NotImplementedException();
        }
    }
}
