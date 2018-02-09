using System;
using System.Threading;
using System.Threading.Tasks;
using BuindingBlocks.Resilience.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Echo.Web.Services
{
    public class EchoService : IEchoService
    {
        private readonly IHttpClient _client;
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _configuration;

        public EchoService(
            IHttpClient client,
            IHttpContextAccessor accessor,
            IConfiguration configuration
            )
        {
            _client = client;
            _accessor = accessor;
            _configuration = configuration;
        }

        public async Task<string> Echo(
            string message,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _client.PutAsync(
                $"{_configuration["EchoApiUrl"]}/api/v1/echo",
                new EchoCommand(message),
                Guid.NewGuid().ToString(),
                await _accessor.HttpContext.GetTokenAsync("access_token"),
                cancellationToken
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to get access to Echo Microservice");
            }

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public class EchoCommand
        {
            public string Input { get; }

            public EchoCommand(
                string input
            )
            {
                Input = input;
            }
        }
    }
}
