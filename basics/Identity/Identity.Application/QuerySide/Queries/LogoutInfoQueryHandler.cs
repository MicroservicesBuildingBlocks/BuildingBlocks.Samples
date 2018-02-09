using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Identity.Application.QuerySide.Queries
{
    public class LogoutInfoQueryHandler : IRequestHandler<LogoutInfoQuery, LogoutInfo>
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogoutInfoQueryHandler(
            IIdentityServerInteractionService interaction,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _interaction = interaction;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LogoutInfo> Handle(LogoutInfoQuery request, CancellationToken cancellationToken)
        {
            var result = new LogoutInfo { LogoutId = request.LogoutId, ShowLogoutPrompt = true };

            var user = _httpContextAccessor.HttpContext.User;
            if (user?.Identity.IsAuthenticated != true)
            {
                result.ShowLogoutPrompt = false;
                return result;
            }

            var context = await _interaction.GetLogoutContextAsync(request.LogoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                result.ShowLogoutPrompt = false;
                return result;
            }

            return result;
        }
    }
}
