using BuildingBlocks.Idempotency;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Echo.Application
{
    public static class Startup
    {
        public static IServiceCollection AddApplication<TRequestManager>(
            this IServiceCollection services, IConfiguration configuration
        ) where TRequestManager : class, IRequestManager
        {
            return services
                .AddSingleton<IRequestManager, TRequestManager>()
                .AddSingleton(ApiInfo.Instantiate(configuration));
        }

        public static IApplicationBuilder UseApplication(
            this IApplicationBuilder app
            )
        {
            return app;
        }
    }
}
