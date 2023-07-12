using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdMultiNetIpInfoHandler : CsProtoStructureHandler<MultiNetIpInfo>
{
    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_MULTI_NET_IPINFO;

    private readonly CharacterManager _characterManager;

    public CsCmdMultiNetIpInfoHandler(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public override void Handle(Client client, MultiNetIpInfo req)
    {
        CsProtoStructurePacket<MultiIspSequenceNtf> rsp = CsProtoResponse.MultiIspSequenceNtf;
        rsp.Structure.Sequence = 0;
        client.SendCsProtoStructurePacket(rsp);

        _characterManager.SendRoleList(client, CsProtoResponse.ListRoleRsp);
    }
}