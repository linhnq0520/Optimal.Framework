using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Optimal.Framework.Infrastructure
{
    public interface IOptimalStartup
    {
        int Order {  get; }
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        void Configure(IApplicationBuilder application);
    }
}
