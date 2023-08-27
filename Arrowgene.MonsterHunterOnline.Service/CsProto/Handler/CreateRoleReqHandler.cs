using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;
using Arrowgene.MonsterHunterOnline.Service.System.CharacterSystem;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CreateRoleReqHandler : CsProtoStructureHandler<RoleCreateInfo>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CreateRoleReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_CREATE_ROLE_REQ;

    private readonly CharacterManager _characterManager;

    public CreateRoleReqHandler(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public override void Handle(Client client, RoleCreateInfo req)
    {
        Logger.Info(client, $"Creating new character:{req.Name} ...");
        Character character = _characterManager.CreateCharacter(client, req);
        if (character == null)
        {
            Logger.Error(client, $"Failed to create new character:{req.Name}");
            CsProtoStructurePacket<RoleDataErrorRsp> err = CsProtoResponse.RoleDataErrorRsp;
            err.Structure.ErrNo = 0;
            client.SendCsProtoStructurePacket(err);

            // TODO cover error cases
            CsProtoStructurePacket<NotifyInfo> notify = CsProtoResponse.NotifyInfo;
            notify.Structure.Info = "Character Name exists, or other error";
            client.SendCsProtoStructurePacket(notify);
            return;
        }

        CsProtoStructurePacket<ListRoleRsp> createRoleRsp = CsProtoResponse.CreateRoleRsp;
        _characterManager.PopulateRoleList(client, createRoleRsp.Structure);
        client.SendCsProtoStructurePacket(createRoleRsp);

        Logger.Info(client, $"Created new character:{req.Name} success");
    }
}