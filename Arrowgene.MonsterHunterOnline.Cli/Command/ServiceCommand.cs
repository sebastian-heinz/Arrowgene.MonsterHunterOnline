using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service;

namespace Arrowgene.MonsterHunterOnline.Cli.Command
{
    public class ServiceCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(ServiceCommand));

        private Server _server;

        public string Key => "service";
        public string Description => "runs mho service";


        public ServiceCommand()
        {
        }

        public CommandResultType Run(CommandParameter parameter)
        {
            if (_server == null)
            {
                _server = new Server(new Setting());
            }

            if (parameter.Arguments.Contains("start"))
            {
                _server.Start();
                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Contains("stop"))
            {
                _server.Stop();
                return CommandResultType.Completed;
            }

            return CommandResultType.Continue;
        }

        public void Shutdown()
        {
            if (_server != null)
            {
                _server.Stop();
            }
        }
    }
}