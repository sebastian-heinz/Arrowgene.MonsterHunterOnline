using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;
using Arrowgene.MonsterHunterOnline.Service.System.CharacterSystem;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class MultiNetIpInfoHandler : CsProtoStructureHandler<MultiNetIpInfo>
{
    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_MULTI_NET_IPINFO;

    private readonly CharacterManager _characterManager;

    public MultiNetIpInfoHandler(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public override void Handle(Client client, MultiNetIpInfo req)
    {
        CsProtoStructurePacket<MultiIspSequenceNtf> multiIspSequenceNtf = CsProtoResponse.MultiIspSequenceNtf;
        multiIspSequenceNtf.Structure.Sequence = 0;
        client.SendCsProtoStructurePacket(multiIspSequenceNtf);

        CsProtoStructurePacket<ListRoleRsp> listRoleRsp = CsProtoResponse.ListRoleRsp;
        _characterManager.PopulateRoleList(client, listRoleRsp.Structure);
        client.SendCsProtoStructurePacket(listRoleRsp);
    }
}