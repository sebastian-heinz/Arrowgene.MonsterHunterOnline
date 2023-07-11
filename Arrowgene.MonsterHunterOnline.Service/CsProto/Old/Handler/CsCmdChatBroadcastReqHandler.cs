using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.System.Chat;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdChatBroadcastReqHandler : ICsProtoHandler
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CsCmdChatBroadcastReqHandler));


    private ChatSystem _chatSystem;

    public CsCmdChatBroadcastReqHandler(ChatSystem chatSystem)
    {
        _chatSystem = chatSystem;
    }

    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_CHAT_BROADCAST_REQ;

    public void Handle(Client client, CsProtoPacket packet)
    {
        // TODO structure seems of, perhaps last entry is uint and not list
        // CSChatBroadcastReq req = new CSChatBroadcastReq();
        // req.Read(packet.NewBuffer());
        // ChatMessage message = new ChatMessage(client, (ChannelType)req.ChannelType, req.Content);
        // _chatSystem.Handle(message);

        IBuffer buf = packet.NewBuffer();
        uint channelType = buf.ReadUInt32(Endianness.Big);
        int contentLen = buf.ReadInt32(Endianness.Big);
        string content = buf.ReadFixedString(contentLen);
        ChatMessage message = new ChatMessage(client, (ChannelType)channelType, content);
        _chatSystem.Handle(message);
    }
}