using System.Collections.Generic;

namespace Arrowgene.MonsterHunterOnline.Service.System.ChatSystem
{
    public interface IChatHandler
    {
        void Handle(Client client, ChatMessage message, List<ChatMessage> responses);
    }
}
