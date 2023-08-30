using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.Database;
using Arrowgene.MonsterHunterOnline.Service.System.CharacterSystem;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class SelectRoleHandler : CsProtoStructureHandler<SelectRoleReq>
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(SelectRoleHandler));


    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_SELECT_ROLE_REQ;

    private readonly CharacterManager _characterManager;
    private readonly ItemManager _itemManager;
    private readonly IDatabase _database;

    public SelectRoleHandler(CharacterManager characterManager, ItemManager itemManager, IDatabase database)
    {
        _characterManager = characterManager;
        _itemManager = itemManager;
        _database = database;
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
        client.Inventory = new Inventory(character.Id, _database);

        CsCsProtoStructurePacket<SelectRoleRsp> selectRoleRsp = CsProtoResponse.SelectRoleRsp;
        selectRoleRsp.Structure.RoleIndex = req.RoleIndex;
        selectRoleRsp.Structure.ErrNo = 0;
        client.SendCsProtoStructurePacket(selectRoleRsp);

        CsCsProtoStructurePacket<TownSessionStart> townSessionStart = CsProtoResponse.TownSessionStart;
        townSessionStart.Structure.ErrNo = 0;
        client.SendCsProtoStructurePacket(townSessionStart);

        CsCsProtoStructurePacket<PlayerInitInfo> playerInitInfo = CsProtoResponse.PlayerInitInfo;
        _characterManager.PopulatePlayerInitInfo(client, client.Character, playerInitInfo.Structure);
        client.SendCsProtoStructurePacket(playerInitInfo);
    }
}