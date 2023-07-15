using System.Collections.Generic;

namespace Arrowgene.MonsterHunterOnline.Service.System.Chat
{
    public class ChatResponse
    {
        public static ChatResponse CommandError(Client client, string message)
        {
            return new ChatResponse()
            {
                Deliver = true,
                Message = message,
                ChannelType = ChannelType.Global,
                Recipients = { client }
            };
        }

        public ChatResponse()
        {
            Recipients = new List<Client>();
            Deliver = true;
            ChannelType = ChannelType.Global;
            Message = "";
        }

        public List<Client> Recipients { get; }
        public bool Deliver { get; set; }
        public ChannelType ChannelType { get; set; }
        public string Message { get; set; }
    }
}