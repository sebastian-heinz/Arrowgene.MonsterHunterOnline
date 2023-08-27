using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;
using Arrowgene.MonsterHunterOnline.Service.System.CharacterSystem;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class ModifyFaceReqHandler : CsProtoStructureHandler<ModifyFaceReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(ModifyFaceReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_MODIFY_FACE_REQ;


    private readonly CharacterManager _characterManager;

    public ModifyFaceReqHandler(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public override void Handle(Client client, ModifyFaceReq req)
    {
        CsProtoStructurePacket<ModifyFaceRsp> modifyFaceRsp = CsProtoResponse.ModifyFaceRsp;
        if (!_characterManager.ModifyCharacter(client, req))
        {
            // TODO error
            modifyFaceRsp.Structure.Result = 0;
            client.SendCsProtoStructurePacket(modifyFaceRsp);
            return;
        }

        modifyFaceRsp.Structure.Result = 0;
        client.SendCsProtoStructurePacket(modifyFaceRsp);
        
        CsProtoStructurePacket<ListRoleRsp> listRoleRsp = CsProtoResponse.ListRoleRsp;
        _characterManager.PopulateRoleList(client, listRoleRsp.Structure);
        client.SendCsProtoStructurePacket(listRoleRsp);
    }
}