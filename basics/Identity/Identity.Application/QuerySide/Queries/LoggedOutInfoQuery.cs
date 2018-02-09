using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Identity.Application.QuerySide.Queries
{
    public class LoggedOutInfoQuery : IRequest<LoggedOutInfo>
    {
        public string LogoutId { get; }

        public LoggedOutInfoQuery(string logoutId)
        {
            LogoutId = logoutId;
        }
    }

    public class LoggedOutInfo
    {
        public string PostLogoutRedirectUri { get; set; }
        public string ClientName { get; set; }
        public string LogoutId { get; set; }
    }
}
