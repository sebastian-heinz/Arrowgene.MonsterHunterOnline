using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System.Chat;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class ChatPrivateReqHandler : CsProtoStructureHandler<ChatPrivateReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(ChatPrivateReqHandler));


    private readonly ChatSystem _chatSystem;

    public ChatPrivateReqHandler(ChatSystem chatSystem)
    {
        _chatSystem = chatSystem;
    }

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_CHAT_PRIVATE_REQ;

    public override void Handle(Client client, ChatPrivateReq req)
    {
        ChatMessage message = new ChatMessage(client, req.ChannelType, req.TargetName, req.Content);
        _chatSystem.Handle(client, message);
    }
}