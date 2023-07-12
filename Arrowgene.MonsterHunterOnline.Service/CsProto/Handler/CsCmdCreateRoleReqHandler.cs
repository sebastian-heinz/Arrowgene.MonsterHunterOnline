using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdCreateRoleReqHandler : CsProtoStructureHandler<RoleCreateInfo>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CsCmdCreateRoleReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_CREATE_ROLE_REQ;

    private readonly CharacterManager _characterManager;

    public CsCmdCreateRoleReqHandler(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public override void Handle(Client client, RoleCreateInfo req)
    {
        Logger.Info(client, $"Creating new character:{req.Name} ...");
        CsProtoStructurePacket<ListRoleRsp> rsp = CsProtoResponse.CreateRoleRsp;

        if (!_characterManager.CreateCharacter(client, req))
        {
            Logger.Error(client, $"Failed to create new character:{req.Name}");
            // TODO maybe
            // <struct name="CSRoleDataErrorRsp" version="1" desc="角色数据错误">
            // <entry name="ErrNo" type="int" desc="0为成功"/>
            // </struct>
           
            //return;
        }

        _characterManager.SendRoleList(client, rsp);
        Logger.Info(client, $"Created new character:{req.Name} success");
    }
}