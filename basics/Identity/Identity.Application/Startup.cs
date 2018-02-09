using System;
using BuildingBlocks.AspnetCoreIdentity.RavenDB;
using Identity.Application.Setup;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application
{
    public static class Startup
    {
        public static IServiceCollection AddIdentityApplication(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.AddDocumentStore(configuration,
                new CollectionMapping(typeof(IdentityResource), "IdentityResources")
                );

            services.AddRavenDBIdentity(
                DocumentStoreHolder.Store,
                DocumentStoreHolder.DatabaseName
            );

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath = "/Accounts/Login";
                options.LogoutPath = "/Accounts/Logout";
                options.AccessDeniedPath = "/Accounts/AccessDenied";
                options.SlidingExpiration = true;
            });

            services
                .AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddRavenDBConfigurationStore(
                    DocumentStoreHolder.Store,
                    DocumentStoreHolder.DatabaseName,
                    async (clientStore, resourceStore) => await BaseConfig.LoadSeed(
                        clientStore, resourceStore, configuration
                        )
                )
                .AddRavenDBOperationalStore(
                    DocumentStoreHolder.Store,
                    DocumentStoreHolder.DatabaseName
                )
                .AddAspNetIdentity<User>()
                .AddProfileService<ProfileService>();

            return services;
        }

        public static IApplicationBuilder UseIdentityApplication(
            this IApplicationBuilder app
        )
        {
            app.UseRavenDBIdentity();
            app.UseAuthentication();
            app.UseIdentityServer();
            return app;
        }
    }
}
