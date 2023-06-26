using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.TQQApi.Handler;

public class TpduCmdNoneHandler : ITpduHandler
{
    private readonly CsProtoOverTpduConsumer _csProtoOverTpduConsumer;

    public TpduCmdNoneHandler(CsProtoOverTpduConsumer csProtoOverTpduConsumer)
    {
        _csProtoOverTpduConsumer = csProtoOverTpduConsumer;
    }

    public TpduCmd Cmd => TpduCmd.TPDU_CMD_NONE;

    public void Handle(Client client, TpduPacket packet)
    {
        _csProtoOverTpduConsumer.HandleReceived(client, packet.Body);
    }
}