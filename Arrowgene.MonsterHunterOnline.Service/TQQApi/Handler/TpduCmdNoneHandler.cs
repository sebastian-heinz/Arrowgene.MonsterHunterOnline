using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.TQQApi.Handler;

public class TpduCmdNoneHandler : ITpduHandler
{
    private readonly CsProtoPacketHandler _csProtoPacketHandler;

    public TpduCmdNoneHandler(CsProtoPacketHandler csProtoPacketHandler)
    {
        _csProtoPacketHandler = csProtoPacketHandler;
    }

    public TpduCmd Cmd => TpduCmd.TPDU_CMD_NONE;

    public void Handle(Client client, TpduPacket packet)
    {
        _csProtoPacketHandler.HandleReceived(client, packet.Body);
    }
}