using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdMultiNetIpInfoHandler : CsProtoStructureHandler<MultiNetIpInfo>
{
    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_MULTI_NET_IPINFO;

    public override void Handle(Client client, MultiNetIpInfo req)
    {
        MultiIspSequenceNtf rsp = new MultiIspSequenceNtf();
        client.SendStructure(CS_CMD_ID.CS_CMD_MULTI_ISP_SEQUENCE_NTF, rsp);

        client.State.OnReady();
    }
}