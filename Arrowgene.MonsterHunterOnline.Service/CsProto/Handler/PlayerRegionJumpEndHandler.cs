using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Protocol.Constant;
using Arrowgene.MonsterHunterOnline.Protocol.Old.ExtraStructures;
using Arrowgene.MonsterHunterOnline.Protocol.Old.Structures;
using Arrowgene.MonsterHunterOnline.Protocol.Structures;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class PlayerRegionJumpEndHandler : CsProtoStructureHandler<PlayerRegionJumpEnd>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(PlayerRegionJumpEnd));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_PLAYER_REGION_JUMP_END;


    public override void Handle(Client client, PlayerRegionJumpEnd req)
    {
        // Spawn a pending combat monster now that the client has finished loading the new region
        if (client.State.PendingMonsterSpawnPos != null)
        {
            CSVec3 monsterPos = client.State.PendingMonsterSpawnPos;
            client.State.PendingMonsterSpawnPos = null;

            // 3-phase spawn protocol (traced from CMonsterSpawner binary):
            // Phase 1: CMD 533 → AddToSpawnQueue → adds to spawn queue at +0x849a8
            // Phase 2: Client sends CMD 534 (LoadEntityReq) back requesting full data
            // Phase 3: Server responds CMD 662 (single MonsterAppearNtf) → SpawnMonsters (type 1)
            //
            // CMD 662 (single) → SpawnMonsters creates PROPER type-1 CMonster_Derived entities
            // CMD 663 (list) → FUN_112a3ac0 creates BROKEN type-8 entities (always crashes)
            //
            // Store spawn info for LoadEntityReqHandler to use in phase 3
            client.State.PendingMonsterSpawnPos = monsterPos;

            uint monsterNetId = 0x10001;

            // Phase 1: Send EntityAppearNtfIdList (CMD 533)
            CsCsProtoStructurePacket<EntityAppearNtfIdList> entityIds = CsProtoResponse.EntityAppearNtfIdList;
            entityIds.Structure.InitType = 0;
            entityIds.Structure.LogicEntityId.Add(monsterNetId);
            entityIds.Structure.LogicEntityType.Add(1); // 1 = Monster
            client.SendCsProtoStructurePacket(entityIds);
        }
    }
}