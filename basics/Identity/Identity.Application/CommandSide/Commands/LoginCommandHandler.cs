using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.AspnetCoreIdentity.RavenDB;
using Identity.Application.Features.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.CommandSide.Commands
{
    class LoginCommandHandler : IRequestHandler<LoginCommand, bool>
    {
        private readonly SignInManager<User> _signInManager;

        public LoginCommandHandler(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<bool> Handle(
            LoginCommand request, 
            CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(
                request.Name, request.Password, request.RememberMe, 
                lockoutOnFailure: false);

            return result.Succeeded;
        }
    }
}
