using System.Collections.Generic;

namespace Arrowgene.MonsterHunterOnline.Service.System.Chat.Command.Commands
{
    /// <summary>
    /// Provides information about the running server version
    /// </summary>
    public class VersionCommand : ChatCommand
    {
        public override AccountType Account => AccountType.User;
        public override string Key => "version";
        public override string HelpText => "usage: `/version` - Provides information about the running server version";

        public override void Execute(string[] command, Client client, ChatMessage message, List<ChatResponse> responses)
        {
            ChatResponse response = new ChatResponse();
            response.Message = Util.GetVersion("Service");
            responses.Add(response);
           // responses.Add(ChatResponse.ServerMessage(client, "Command Executed"));
        }
    }
}