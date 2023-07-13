using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdDeleteRoleReqHandler : CsProtoStructureHandler<DeleteRoleReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CsCmdDeleteRoleReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_DELETE_ROLE_REQ;

    private readonly CharacterManager _characterManager;

    public CsCmdDeleteRoleReqHandler(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public override void Handle(Client client, DeleteRoleReq req)
    {
        Logger.Info(client, $"Deleting character:{req.RoleIndex} ...");
        if (!_characterManager.DeleteCharacter(client, (byte)req.RoleIndex))
        {
            Logger.Info(client, $"Failed to delete character:{req.RoleIndex}");
        }

        Logger.Info(client, $"Deleted character:{req.RoleIndex}");
    }
}