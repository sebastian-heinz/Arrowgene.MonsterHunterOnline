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

            int monsterNetId = 1000;

            // Use CtrledMonsterAppearNtf — the battle-active monster packet from SpawnCommand "battle" case
            CsCsProtoStructurePacket<CtrledMonsterAppearNtf> battle = CsProtoResponse.CtrledMonsterAppearNtf;
            battle.Structure.BaseInfo = new MonsterAppearNtf()
            {
                NetId = monsterNetId,
                SpawnType = 1,
                MonsterInfoId = 50080,
                EntGuid = 0,
                Name = "",
                Class = "",
                Pose = new CSQuatT(monsterPos, new CSQuat(1.0f, 0, 0, 0)),
                Faction = 0,
                Dead = 0,
                ParentGuid = 0,
                LastChildId = 0,
                LcmState = new CSMonsterLocomotion()
                {
                    MonsterID = (uint)monsterNetId,
                    MonsterPos = monsterPos,
                    MonsterRot = new CSQuat(1.0f, 0, 0, 0),
                    TargetSrvID = 1,
                },
                BBVars = new CSBBVarList() { Vars = new List<CSBBVar>() { new CSBBVar("ExFlag", new CSBBBool(true)) } },
            };
            battle.Structure.OwnerId = (int)client.Character.Id;
            battle.Structure.Type = 1;
            battle.Structure.Duration = 0.0f;
            client.SendCsProtoStructurePacket(battle);
        }
    }
}