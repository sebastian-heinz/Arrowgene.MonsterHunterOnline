using System.Collections.Generic;

namespace Arrowgene.MonsterHunterOnline.Service.System.ChatSystem.Command.Commands
{
    /// <summary>
    /// Provides information about the running server version
    /// </summary>
    public class VersionCommand : ChatCommand
    {
        public override AccountType Account => AccountType.User;
        public override string Key => "version";
        public override string HelpText => "usage: `/version` - Provides information about the running server version";

        public override void Execute(string[] command, Client client, ChatMessage message, List<ChatMessage> responses)
        {
            string msg = Util.GetVersion("Service");
            ChatMessage response = ChatMessage.CommandMessage(client, msg);
            responses.Add(response);
        }
    }
}