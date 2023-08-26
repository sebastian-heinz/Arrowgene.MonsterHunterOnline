using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class SelectRoleHandler : CsProtoStructureHandler<SelectRoleReq>
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(SelectRoleHandler));


    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_SELECT_ROLE_REQ;

    private readonly CharacterManager _characterManager;

    public SelectRoleHandler(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public override void Handle(Client client, SelectRoleReq req)
    {
        Character character = _characterManager.GetCharacter(client.Account.Id, (byte)req.RoleIndex);
        if (character == null)
        {
            Logger.Error(client, $"character for index:{req.RoleIndex} not found");
            return;
        }

        client.Character = character;
        
        // TODO load inventory
        client.Inventory = new Inventory();

        CsProtoStructurePacket<SelectRoleRsp> selectRoleRsp = CsProtoResponse.SelectRoleRsp;
        selectRoleRsp.Structure.RoleIndex = req.RoleIndex;
        selectRoleRsp.Structure.ErrNo = 0;
        client.SendCsProtoStructurePacket(selectRoleRsp);

        CsProtoStructurePacket<TownSessionStart> townSessionStart = CsProtoResponse.TownSessionStart;
        townSessionStart.Structure.ErrNo = 0;
        client.SendCsProtoStructurePacket(townSessionStart);

        CsProtoStructurePacket<PlayerInitInfo> playerInitInfo = CsProtoResponse.PlayerInitInfo;
        _characterManager.PopulatePlayerInitInfo(client, client.Character, playerInitInfo.Structure);
        client.SendCsProtoStructurePacket(playerInitInfo);
    }
}