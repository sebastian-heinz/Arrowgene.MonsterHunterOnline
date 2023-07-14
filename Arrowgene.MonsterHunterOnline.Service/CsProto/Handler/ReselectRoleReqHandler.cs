using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class ReselectRoleReqHandler : CsProtoStructureHandler<ReselectRoleReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(ReselectRoleReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_RESELECT_ROLE_REQ;

    private readonly CharacterManager _characterManager;

    public ReselectRoleReqHandler(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public override void Handle(Client client, ReselectRoleReq req)
    {
        CsProtoStructurePacket<ReselectRoleRsp> reselectRoleRsp = CsProtoResponse.ReselectRoleRsp;
        client.SendCsProtoStructurePacket(reselectRoleRsp);

        CsProtoStructurePacket<ListRoleRsp> listRoleRsp = CsProtoResponse.ListRoleRsp;
        _characterManager.PopulateRoleList(client, listRoleRsp.Structure);
        client.SendCsProtoStructurePacket(listRoleRsp);
    }
}