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
        CsItemRebuildLimitInfo rsp = new CsItemRebuildLimitInfo();
        client.SendCsProto(rsp.BuildPacket());
    }
}