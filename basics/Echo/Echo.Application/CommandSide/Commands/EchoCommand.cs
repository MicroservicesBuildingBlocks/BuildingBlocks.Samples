using BuildingBlocks.Mediatr.Commands;

namespace Echo.Application.CommandSide.Commands
{
    public class EchoCommand : ICommand<string>
    {
        public string Input { get; }

        public EchoCommand(
            string input
            )
        {
            Input = input;
        }
    }
}
