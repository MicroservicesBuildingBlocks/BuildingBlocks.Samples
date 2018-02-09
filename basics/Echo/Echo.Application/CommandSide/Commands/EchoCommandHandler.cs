using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Echo.Application.CommandSide.Commands
{
    public class EchoCommandHandler
        : IRequestHandler<EchoCommand, string>
    {
        public Task<string> Handle(
            EchoCommand request, 
            CancellationToken cancellationToken
            )
        {
            return Task.FromResult(request.Input);
        }
    }
}
