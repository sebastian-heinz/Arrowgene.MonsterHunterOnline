using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.Database;
using Arrowgene.MonsterHunterOnline.Service.System;
using Arrowgene.MonsterHunterOnline.Service.System.CharacterSystem;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class InstanceVerifyReqHandler : CsProtoStructureHandler<InstanceVerifyReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(InstanceVerifyReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_INSTANCE_VERIFY_REQ;

    private readonly CharacterManager _characterManager;

    private readonly IDatabase _database;

    public InstanceVerifyReqHandler(CharacterManager characterManager, IDatabase database)
    {
        _characterManager = characterManager;
        _database = database;
    }

    public override void Handle(Client client, InstanceVerifyReq req)
    {
        Logger.Trace(client, req.JsonDump());


        Account account = _database.SelectAccountByUin(req.Uin);
        if (account == null)
        {
            Logger.Error(client, $"account == null (Uin:{req.Uin})");
            return;
        }


        Character character = _database.SelectCharacterById((uint)req.RoleId);
        if (character == null)
        {
            Logger.Error(client, $"character == null (RoleId:{req.RoleId})");
            return;
        }

        if (account.Id != character.AccountId)
        {
            Logger.Error(client,
                $"account.Id({account.Id}) != character.AccountId({character.AccountId}) (CharacterId:{character.Id})");
            return;
        }

        client.Account = account;
        client.Character = character;
        client.Inventory = new Inventory(character.Id, _database);

        CsCsProtoStructurePacket<PlayerInitInfo> playerInitInfo = CsProtoResponse.PlayerInitInfo;
        playerInitInfo.Structure.Pose.t.x = 1681.2958f;
        playerInitInfo.Structure.Pose.t.y = 346.80392f;
        playerInitInfo.Structure.Pose.t.z = 205.375f;
        _characterManager.PopulatePlayerInitInfo(client, client.Character, playerInitInfo.Structure);
        client.SendCsProtoStructurePacket(playerInitInfo);

        CsCsProtoStructurePacket<InstanceVerifyRsp> instanceVerifyRsp = CsProtoResponse.InstanceVerifyRsp;
        client.SendCsProtoStructurePacket(instanceVerifyRsp);

        // TODO 
        // CsCsProtoStructurePacket<PlayerTeleport> PlayerTeleport = CsProtoResponse.PlayerTeleport;
        // PlayerTeleport.Structure.SyncTime = 1;
        // PlayerTeleport.Structure.NetObjId = client.Character.Id;
        // PlayerTeleport.Structure.Region = client.State.MainInstanceLevelId;
        // PlayerTeleport.Structure.TargetPos.t.x = 1681.2958f;
        // PlayerTeleport.Structure.TargetPos.t.y = 346.80392f;
        // PlayerTeleport.Structure.TargetPos.t.z = 205.375f;
        // PlayerTeleport.Structure.ParentGuid = 1;
        // PlayerTeleport.Structure.InitState = 1;
        // client.SendCsProtoStructurePacket(PlayerTeleport);
    }
}