using MediatR;

namespace Identity.Application.QuerySide.Queries
{
    public class LogoutInfoQuery : IRequest<LogoutInfo>
    {
        public string LogoutId { get; }

        public LogoutInfoQuery(string logoutId)
        {
            LogoutId = logoutId;
        }
    }

    public class LogoutInfo
    {
        public string LogoutId { get; set; }
        public bool ShowLogoutPrompt { get; set; }
    }
}
