using BuildingBlocks.Mediatr.Commands;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.CommandSide.Commands
{
    public class RegisterCommand : ICommand<IdentityResult>
    {
        public string Name { get; }
        public string Password { get; }

        public RegisterCommand(
            string name,
            string password)
        {
            Name = name;
            Password = password;
        }
    }
}
