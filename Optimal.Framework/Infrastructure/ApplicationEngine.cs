using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Optimal.Framework.Infrastructure
{
    public class ApplicationEngine:IEngine
    {
        public virtual IServiceProvider ServiceProvider { get; protected set; }

        protected IServiceProvider GetServiceProvider(IServiceScope scope = null)
        {
            if (scope == null)
            {
                return ((ServiceProvider?.GetService<IHttpContextAccessor>())?.HttpContext)?.RequestServices ?? ServiceProvider;
            }
            return scope.ServiceProvider;
        }

        protected virtual void RunStartupTasks()
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton((IEngine)this);
            foreach (IApplicationStartup item in from startup in Singleton<ITypeFinder>.Instance.FindClassesOfType<IApplicationStartup>()
                                             select (IApplicationStartup)Activator.CreateInstance(startup) into startup
                                             orderby startup.Order
                                             select startup)
            {
                item.ConfigureServices(services, configuration);
            }
            services.AddSingleton(services);
            RunStartupTasks();
            ServiceProvider = services.BuildServiceProvider();
            EngineContext.Replace(this);
        }

        public T Resolve<T>(IServiceScope scope = null) where T : class
        {
            return (T)Resolve(typeof(T), scope);
        }

        public object Resolve(Type type, IServiceScope scope = null)
        {
            return GetServiceProvider(scope)?.GetService(type);
        }

        public virtual IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
        }

        public virtual object ResolveUnregistered(Type type)
        {
            Exception innerException = null;
            ConstructorInfo[] constructors = type.GetConstructors();
            foreach (ConstructorInfo constructor in constructors)
            {
                try
                {
                    IEnumerable<object> parameters = from parameter in constructor.GetParameters()
                                                     select Resolve(parameter.ParameterType) ?? throw new Exception("Unknown dependency");
                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }
            throw new Exception("No constructor was found for " + type.FullName + ".", innerException);
        }
    }
}
