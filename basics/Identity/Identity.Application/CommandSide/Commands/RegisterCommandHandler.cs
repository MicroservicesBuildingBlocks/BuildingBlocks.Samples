using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.AspnetCoreIdentity.RavenDB;
using Identity.Application.CommandSide.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Features.Account
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IdentityResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegisterCommandHandler(
            UserManager<User> userManager,
            SignInManager<User> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> Handle(
            RegisterCommand request, 
            CancellationToken cancellationToken
            )
        {
            var user = new User { Name = request.Name };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return result;
        }
    }
}
