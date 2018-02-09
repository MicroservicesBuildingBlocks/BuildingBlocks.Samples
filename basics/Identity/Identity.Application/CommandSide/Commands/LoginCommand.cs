using BuildingBlocks.Mediatr.Commands;

namespace Identity.Application.Features.Account
{
    public class LoginCommand : ICommand<bool>
    {
        public string Name { get; }
        public string Password { get; }
        public bool RememberMe { get; }

        public LoginCommand(
            string name,
            string password,
            bool rememberMe)
        {
            Name = name;
            Password = password;
            RememberMe = rememberMe;
        }
    }
}
