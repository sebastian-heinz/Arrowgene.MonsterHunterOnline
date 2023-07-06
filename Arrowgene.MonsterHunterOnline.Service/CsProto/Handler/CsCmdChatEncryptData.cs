using System;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.System.Chat;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdChatEncryptData : ICsProtoHandler
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(CsCmdChatEncryptData));

    private static string Key = "GetSystemDirecto";

    private ChatSystem _chatSystem;
    private CsProtoPacketHandler _csProtoPacketHandler;

    public CsCmdChatEncryptData(CsProtoPacketHandler csProtoPacketHandler, ChatSystem chatSystem)
    {
        _csProtoPacketHandler = csProtoPacketHandler;
        _chatSystem = chatSystem;
    }

    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_CHAT_ENCRYPT_DATA;

    public void Handle(Client client, CsProtoPacket packet)
    {
        IBuffer req = new StreamBuffer(packet.Body);
        req.SetPositionStart();

        uint encSize = req.ReadUInt32(Endianness.Big);
        byte[] encData = req.ReadBytes((int)encSize);
        byte[] decData = new byte[encData.Length];

        for (int i = 0; i < encData.Length; i++)
        {
            decData[i] = (byte)(Key[i % Key.Length] ^ encData[i]);
        }

        Logger.Info(client, $"Decrypted Chat Data:{Environment.NewLine}{Util.HexDump(decData)}");
        _csProtoPacketHandler.HandleReceived(client, decData);

        return;

        // TODO it seemed like i received unwrapped chat messages sometimes

        StreamBuffer buf = new StreamBuffer(decData);
        buf.SetPositionStart();
        uint channelType = buf.ReadUInt32(Endianness.Big);
        int contentLen = buf.ReadInt32(Endianness.Big);
        string content = buf.ReadString(contentLen);

        ChatMessage message = new ChatMessage(client, (ChannelType)channelType, content);
        _chatSystem.Handle(message);


        // CSChatBroadcastReq
        // 	<struct name="CSChatBroadcastReq" version="1" desc="客户端发送的广播聊天请求">
        // <entry name="ChannelType" type="int" desc="频道类型ID"/>
        //     <entry name="Content" type="string" size="MAX_CHAT_CONTENT_LEN" desc="聊天内容" sizeinfo="int"/>
        //     <entry name="Items" type="CsChatItemsPkg" desc="聊天道具数据"/>
        //     </struct>
    }
}