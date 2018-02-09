using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;

using BuildingBlocks.Core;
using BuildingBlocks.Idempotency;
using BuildingBlocks.Mediatr.Autofac;
using BuildingBlocks.Mediatr.Exceptions;
using BuildingBlocks.Swagger;

using Echo.Application;

namespace Echo.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                });

            services.AddHealthChecks(checks =>
            {
                checks.AddUrlCheck($"{Configuration["AuthenticationAuthorityUrl"]}/hc");
            });

            services
                .AddApplication<InMemoryRequestManager>(Configuration)
                .AddPermissiveCors()
                .AddCustomIdentity(ApiInfo.Instance)
                .AddCustomSwagger(ApiInfo.Instance);

            return services.ConvertToAutofac(
                MediatrModule.Create(ApiInfo.Instance.ApplicationAssembly)
                );
        }
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApiInfo apiInfo) => app
                .UseDeveloperExceptionPage()
                .UseApplication()
                .UsePermissiveCors()
                .UseCustomSwagger(apiInfo)
                .UseAuthentication()
                .UseMvcWithDefaultRoute();
    }
}
