using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Optimal.Framework.Infrastructure
{
    public interface IEngine
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        T Resolve<T>(IServiceScope scope = null) where T : class;

        object Resolve(Type type, IServiceScope scope = null);

        IEnumerable<T> ResolveAll<T>();

        object ResolveUnregistered(Type type);
    }
}
