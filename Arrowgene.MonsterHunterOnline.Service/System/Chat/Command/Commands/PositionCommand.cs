using System.Collections.Generic;

namespace Arrowgene.MonsterHunterOnline.Service.System.Chat.Command.Commands
{
    /// <summary>
    /// prints current position to chat
    /// </summary>
    public class PositionCommand : ChatCommand
    {
        public override AccountType Account => AccountType.User;
        public override string Key => "pos";
        public override string HelpText => "usage: `/pos` - prints current position to chat";

        public override void Execute(string[] command, Client client, ChatMessage message, List<ChatMessage> responses)
        {
            string msg = $"X:{client.State.Position.x} Y:{client.State.Position.y} Z:{client.State.Position.z}";
            ChatMessage response = ChatMessage.CommandMessage(client, msg);
            responses.Add(response);
        }
    }
}