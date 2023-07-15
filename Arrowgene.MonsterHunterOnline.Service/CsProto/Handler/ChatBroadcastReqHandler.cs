using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System.Chat;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class ChatBroadcastReqHandler : CsProtoStructureHandler<ChatBroadcastReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(ChatBroadcastReqHandler));


    private readonly ChatSystem _chatSystem;

    public ChatBroadcastReqHandler(ChatSystem chatSystem)
    {
        _chatSystem = chatSystem;
    }

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_CHAT_BROADCAST_REQ;

    public override void Handle(Client client, ChatBroadcastReq req)
    {
        // TODO structure seems of, perhaps last entry is uint and not list
        // CSChatBroadcastReq req = new CSChatBroadcastReq();
        // req.Read(packet.NewBuffer());
        // ChatMessage message = new ChatMessage(client, (ChannelType)req.ChannelType, req.Content);
        // _chatSystem.Handle(message);

        ChatMessage message = new ChatMessage(client, req.ChannelType, req.Content);
        _chatSystem.Handle(client, message);
    }
}