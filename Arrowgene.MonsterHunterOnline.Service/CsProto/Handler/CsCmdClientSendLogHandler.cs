using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdClientSendLogHandler : ICsProtoHandler
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(CsCmdClientSendLogHandler));


    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_CLIENT_SEND_LOG;

    public void Handle(Client client, CsProtoPacket packet)
    {
        IBuffer req = new StreamBuffer(packet.Body);
        req.SetPositionStart();
        CS_CLIENTLOG_REASON reason = (CS_CLIENTLOG_REASON)req.ReadInt32(Endianness.Big);
        int logSize = req.ReadInt32(Endianness.Big);
        if (logSize > CsProtoConstant.MAX_CHAT_CONTENT_LEN)
        {
            logSize = CsProtoConstant.MAX_CHAT_CONTENT_LEN;
        }

        string log = req.ReadString(logSize - 1);
        Logger.Info(client, $"{reason}:{log}");
    }
}