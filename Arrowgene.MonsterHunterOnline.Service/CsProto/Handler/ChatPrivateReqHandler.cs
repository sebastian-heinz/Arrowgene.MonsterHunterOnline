using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System.ChatSystem;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class ChatPrivateReqHandler : CsProtoStructureHandler<ChatPrivateReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(ChatPrivateReqHandler));


    private readonly ChatManager _chatManager;

    public ChatPrivateReqHandler(ChatManager chatManager)
    {
        _chatManager = chatManager;
    }

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_CHAT_PRIVATE_REQ;

    public override void Handle(Client client, ChatPrivateReq req)
    {
        ChatMessage message = new ChatMessage(client, req.ChannelType, req.TargetName, req.Content);
        _chatManager.Handle(client, message);
    }
}