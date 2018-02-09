using System.Collections.Generic;
using System.Reflection;
using BuildingBlocks.Core;
using Microsoft.Extensions.Configuration;

namespace Echo.Application
{
    public class ApiInfo : IApiInfo
    {
        private ApiInfo(IConfiguration config)
        {
            AuthenticationAuthority = config["AuthenticationAuthorityUrl"];
        }
        public string AuthenticationAuthority { get; }
        public string JwtBearerAudience => "echoapi";
        public string Code => "echoapi";
        public string Title => "Echo Api";
        public string Version => "V1";
        public Assembly ApplicationAssembly => GetType().Assembly;

        public IDictionary<string, string> Scopes => new Dictionary<string, string>
        {
            {"echoapi", Title}
        };

        public SwaggerAuthInfo SwaggerAuthInfo => new SwaggerAuthInfo(
            "echoapiswaggerui", "", ""
            );

        public static IApiInfo Instantiate(IConfiguration config)
        {
            Instance = new ApiInfo(config);
            return Instance;
        }
        public static IApiInfo Instance { get; private set; }
   }
}
