using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Identity.Application.CommandSide.Commands
{
    class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        public Task Handle(LogoutCommand message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
