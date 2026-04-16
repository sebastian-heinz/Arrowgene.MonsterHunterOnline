using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Protocol.Constant;
using Arrowgene.MonsterHunterOnline.Protocol.Structures;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Protocol.Old.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class LoadEntityReqHandler : CsProtoStructureHandler<LoadEntityReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(LoadEntityReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_LOAD_ENTITY_REQ;


    public LoadEntityReqHandler()
    {
    }

    public override void Handle(Client client, LoadEntityReq req)
    {
        Logger.Debug(req.JsonDump());

        // Phase 3 of the 3-phase spawn protocol:
        // Client sent CMD 534 (LoadEntityReq) requesting full entity data
        // after we sent CMD 533 (EntityAppearNtfIdList) in Phase 1.
        // Respond with CMD 662 (SINGLE MonsterAppearNtf, NOT list CMD 663!)
        // CMD 662 → CMonsterSpawner::SpawnMonsters → type-1 CMonster_Derived (proper entity)
        // CMD 663 → FUN_112a3ac0 → type-8 entity (BROKEN, crashes)

        for (int i = 0; i < req.LogicEntityId.Count; i++)
        {
            uint netId = req.LogicEntityId[i];
            uint entityType = req.LogicEntityType[i];

            CSVec3 spawnPos = client.State.PendingMonsterSpawnPos ?? client.State.Position;

            // Send CMD 662 (single MonsterAppearNtf) for CMonsterSpawner type-1 entity
            CsCsProtoStructurePacket<MonsterAppearNtf> monsterAppearNtf = CsProtoResponse.MonsterAppearNtf;
            monsterAppearNtf.Structure.NetId = (int)netId;
            monsterAppearNtf.Structure.SpawnType = 1;
            monsterAppearNtf.Structure.MonsterInfoId = 50080;
            monsterAppearNtf.Structure.EntGuid = 12345;
            monsterAppearNtf.Structure.Name = "M008_RaptorCrimson";
            monsterAppearNtf.Structure.Class = "";
            monsterAppearNtf.Structure.Pose = new CSQuatT(spawnPos, new CSQuat(1.0f, 0, 0, 0));
            monsterAppearNtf.Structure.Faction = 2;
            monsterAppearNtf.Structure.BTState = "Idle";
            monsterAppearNtf.Structure.Dead = 0;
            monsterAppearNtf.Structure.ParentGuid = 0;
            monsterAppearNtf.Structure.LastChildId = -1;
            monsterAppearNtf.Structure.LcmState.MonsterID = netId;
            monsterAppearNtf.Structure.LcmState.AnimSeqName = "Idle";
            monsterAppearNtf.Structure.LcmState.MonsterPos = spawnPos;
            monsterAppearNtf.Structure.LcmState.MonsterRot = new CSQuat(1.0f, 0, 0, 0);
            monsterAppearNtf.Structure.LcmState.TargetSrvID = 1;
            client.SendCsProtoStructurePacket(monsterAppearNtf);

            // ALSO send render-only spawn via CMD 663 with NetId=0 (proven to render model)
            // This is a fallback in case SpawnMonsters can't load the model from CMonsterInfo
            CsCsProtoStructurePacket<MonsterAppearNtfList> renderSpawn = CsProtoResponse.MonsterAppearNtfList;
            renderSpawn.Structure.Appear.Add(new MonsterAppearNtf()
            {
                NetId = 0,
                SpawnType = 1,
                MonsterInfoId = 50080,
                Pose = new CSQuatT(spawnPos, new CSQuat(1.0f, 0, 0, 0)),
            });
            client.SendCsProtoStructurePacket(renderSpawn);

            // Send MonsterActiveState (CMD 528) to activate the type-1 entity
            CsCsProtoStructurePacket<MonsterActiveState> activeState = CsProtoResponse.MonsterActiveState;
            activeState.Structure.SyncTime = 0;
            activeState.Structure.ActiveState = 1;
            activeState.Structure.MonsterId = netId;
            activeState.Structure.Position = new XYZPosition() { x = spawnPos.x, y = spawnPos.y, z = spawnPos.z };
            activeState.Structure.Rotation = new Quaternion() { x = 0, y = 0, z = 0, w = 1 };
            client.SendCsProtoStructurePacket(activeState);
        }
    }
}