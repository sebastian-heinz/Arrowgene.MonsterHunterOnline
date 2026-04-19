using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Protocol.Constant;
using Arrowgene.MonsterHunterOnline.Protocol.Old.ExtraStructures;
using Arrowgene.MonsterHunterOnline.Protocol.Old.Structures;
using Arrowgene.MonsterHunterOnline.Protocol.Structures;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class PlayerRegionJumpEndHandler : CsProtoStructureHandler<PlayerRegionJumpEnd>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(PlayerRegionJumpEnd));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_PLAYER_REGION_JUMP_END;


    public override void Handle(Client client, PlayerRegionJumpEnd req)
    {
        if (client.State.PendingMonsterSpawnPos == null)
        {
            return;
        }

        CSVec3 monsterPos = client.State.PendingMonsterSpawnPos;
        client.State.PendingMonsterSpawnPos = null;
        client.State.PendingMonsterNetId = null;

        uint monsterNetId = 1000;
        CSQuat monsterRot = new(1.0f, 0, 0, 0);

        // CMD 714 renders the monster shell in the current client state.
        // Follow it immediately with the runtime state packets that normally
        // drive animation and movement updates.
        CsCsProtoStructurePacket<CtrledMonsterAppearNtf> battle = CsProtoResponse.CtrledMonsterAppearNtf;
        battle.Structure.BaseInfo = new MonsterAppearNtf()
        {
            NetId = (int)monsterNetId,
            SpawnType = 1,
            MonsterInfoId = 60030,
            EntGuid = 0,
            Name = "",
            Class = "",
            Pose = new CSQuatT(monsterPos, monsterRot),
            Faction = 0,
            Dead = 0,
            ParentGuid = 0,
            LastChildId = 0,
            LcmState = new CSMonsterLocomotion()
            {
                MonsterID = monsterNetId,
                AnimSeqName = "Idle",
                MonsterPos = monsterPos,
                MonsterRot = monsterRot,
                SkillSpeed = 1.0f,
                RestartAnim = 1,
                SetPos = 1,
                SetRotate = 1,
            },
            BTState = "Em025\\BB_Knowledge.xml",
            BBVars = new CSBBVarList()
            {
                Vars = new List<CSBBVar>() { new CSBBVar("ExFlag", new CSBBBool(true)) }
            },
        };
        battle.Structure.OwnerId = 1;
        battle.Structure.Type = 1;
        battle.Structure.Duration = 0.0f;
        client.SendCsProtoStructurePacket(battle);

        CsCsProtoStructurePacket<MonsterActiveState> activeState = CsProtoResponse.MonsterActiveState;
        activeState.Structure.SyncTime = 0;
        activeState.Structure.ActiveState = 1;
        activeState.Structure.MonsterId = monsterNetId;
        activeState.Structure.Position = new XYZPosition() { x = monsterPos.x, y = monsterPos.y, z = monsterPos.z };
        activeState.Structure.Rotation = new Quaternion() { x = 0, y = 0, z = 0, w = 1 };
        client.SendCsProtoStructurePacket(activeState);

        CsCsProtoStructurePacket<MonsterSequenceState> seq = CsProtoResponse.MonsterSequenceState;
        seq.Structure.MonsterID = monsterNetId;
        seq.Structure.AnimSeqName = "Idle";
        seq.Structure.CurTime = 0.0f;
        seq.Structure.Location = monsterPos;
        seq.Structure.Rotation = monsterRot;
        client.SendCsProtoStructurePacket(seq);

        CSMonsterLocomotion locomotion = new()
        {
            SyncTime = 0,
            MonsterID = monsterNetId,
            AnimSeqName = "Idle",
            MonsterPos = monsterPos,
            MonsterRot = monsterRot,
            SkillSpeed = 1.0f,
            RestartAnim = 1,
            SetPos = 1,
            SetRotate = 1,
        };
        client.SendCsPacket(NewCsPacket.MonsterLCM(locomotion));
    }
}
