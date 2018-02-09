using System.Threading;
using System.Threading.Tasks;

namespace Echo.Web.Services
{
    public interface IEchoService
    {
        Task<string> Echo(string message,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}