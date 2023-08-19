using System.Collections.Generic;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.Service.System.Chat.Log
{
    public class ChatLogHandler : IChatHandler
    {
        private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(ChatLogHandler));


        public ChatLogHandler()
        {
        }

        public void Handle(Client client, ChatMessage message, List<ChatMessage> responses)
        {
            Logger.Info(client, message.Message);
            ChatMessageLogEntry logEntry = new ChatMessageLogEntry(client.Character, message);
        }
    }
}