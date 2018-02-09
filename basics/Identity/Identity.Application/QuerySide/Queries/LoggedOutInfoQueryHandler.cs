using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Services;
using MediatR;

namespace Identity.Application.QuerySide.Queries
{
    public class LoggedOutInfoQueryHandler :
        IRequestHandler<LoggedOutInfoQuery, LoggedOutInfo>
    {
        private readonly IIdentityServerInteractionService _interaction;

        public LoggedOutInfoQueryHandler(
            IIdentityServerInteractionService interaction
        )
        {
            _interaction = interaction;
        }

        public async Task<LoggedOutInfo> Handle(
            LoggedOutInfoQuery request, 
            CancellationToken cancellationToken)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(request.LogoutId);

            var vm = new LoggedOutInfo
            {
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = logout?.ClientId,
                LogoutId = request.LogoutId
            };

            return vm;
        }
    }
}
