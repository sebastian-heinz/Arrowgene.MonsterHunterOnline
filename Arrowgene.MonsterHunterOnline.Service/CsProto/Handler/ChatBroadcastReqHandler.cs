using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System.ChatSystem;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class ChatBroadcastReqHandler : CsProtoStructureHandler<ChatBroadcastReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(ChatBroadcastReqHandler));


    private readonly ChatManager _chatManager;

    public ChatBroadcastReqHandler(ChatManager chatManager)
    {
        _chatManager = chatManager;
    }

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_CHAT_BROADCAST_REQ;

    public override void Handle(Client client, ChatBroadcastReq req)
    {
        ChatMessage message = new ChatMessage(client, req.ChannelType, req.Content);
        _chatManager.Handle(client, message);
    }
}