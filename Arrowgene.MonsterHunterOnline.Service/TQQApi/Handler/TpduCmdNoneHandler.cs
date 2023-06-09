using Arrowgene.MonsterHunterOnline.Service.CsProto;

namespace Arrowgene.MonsterHunterOnline.Service.TQQApi.Handler;

public class TpduCmdNoneHandler : ITpduHandler
{
    private readonly CsProtoConsumer _csProtoConsumer;

    public TpduCmdNoneHandler(CsProtoConsumer csProtoConsumer)
    {
        _csProtoConsumer = csProtoConsumer;
    }

    public TpduCmd Cmd => TpduCmd.TPDU_CMD_NONE;

    public void Handle(Client client, TpduPacket packet)
    {
        _csProtoConsumer.HandleReceived(client, packet.Body);
    }
}