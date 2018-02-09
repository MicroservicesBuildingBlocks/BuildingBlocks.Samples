using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.AspnetCoreIdentity.RavenDB;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.CommandSide.Commands
{
    class LogoffCommandHandler : IRequestHandler<LogoffCommand>
    {
        private readonly SignInManager<User> _signInManager;

        public LogoffCommandHandler(
            SignInManager<User> signInManager
            )
        {
            _signInManager = signInManager;
        }

        public Task Handle(LogoffCommand message, CancellationToken cancellationToken) => 
            _signInManager?.SignOutAsync();
    }
}
