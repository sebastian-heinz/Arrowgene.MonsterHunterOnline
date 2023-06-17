using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdItemReBuildLimitDataHandler : ICsProtoHandler
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CsCmdItemReBuildLimitDataHandler));

    public CsProtoCmd Cmd => CsProtoCmd.CS_CMD_ITEMREBUILD_LIMITDATA_REQ;

    public void Handle(Client client, CsProtoPacket packet)
    {
        ItemRebuildLimitDataNtf rsp = new ItemRebuildLimitDataNtf();
        client.SendCsProto(rsp.BuildPacket());
    }
}