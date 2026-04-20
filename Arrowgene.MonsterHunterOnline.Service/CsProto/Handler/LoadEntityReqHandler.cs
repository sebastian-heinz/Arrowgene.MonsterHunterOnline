using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Protocol.Constant;
using Arrowgene.MonsterHunterOnline.Protocol.Old.Structures;
using Arrowgene.MonsterHunterOnline.Protocol.Structures;
using Arrowgene.MonsterHunterOnline.Service.CsProto;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.System;
using System;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class LoadEntityReqHandler : CsProtoStructureHandler<LoadEntityReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(LoadEntityReqHandler));

    private const int BattleMonsterInfoId = 60030;

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_LOAD_ENTITY_REQ;

    public override void Handle(Client client, LoadEntityReq req)
    {
        Logger.Debug(req.JsonDump());

        if (client.State.PendingMonsterSpawnPos == null || client.State.PendingMonsterNetId == null)
        {
            return;
        }

        uint pendingMonsterNetId = client.State.PendingMonsterNetId.Value;
        CSVec3 spawnPos = client.State.PendingMonsterSpawnPos;
        int count = Math.Min(req.LogicEntityId.Count, req.LogicEntityType.Count);

        for (int i = 0; i < count; i++)
        {
            uint netId = req.LogicEntityId[i];
            uint entityType = req.LogicEntityType[i];

            if (netId != pendingMonsterNetId || entityType != (uint)LogicEntityType.MH_LETYPE_MONSTER)
            {
                continue;
            }

            Logger.Info(client,
                $"Receive CMD 534 for battle monster NetId=0x{netId:X8} Type={entityType}; sending runtime-verified CMD 663 count=1");

            long syncTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            CSQuat monsterRot = new(1.0f, 0, 0, 0);

            MonsterAppearNtf monster = new()
            {
                NetId = (int)netId,
                SpawnType = 1,
                MonsterInfoId = BattleMonsterInfoId,
                EntGuid = netId,
                Name = string.Empty,
                Class = string.Empty,
                Pose = new CSQuatT(spawnPos, monsterRot),
                Faction = 2,
                BTState = "Idle",
                Dead = 0,
                ParentGuid = 0,
                LastChildId = -1,
                LcmState = new CSMonsterLocomotion
                {
                    SyncTime = syncTime,
                    MonsterID = netId,
                    AnimSeqName = "Idle",
                    MonsterPos = spawnPos,
                    MonsterRot = monsterRot,
                    SkillSpeed = 1.0f,
                    RestartAnim = 1,
                    SetPos = 1,
                    SetRotate = 1,
                },
            };

            CsCsProtoStructurePacket<MonsterAppearNtfList> monsterAppearList = CsProtoResponse.MonsterAppearNtfList;
            monsterAppearList.Structure.Appear.Add(monster);

            Logger.Info(client,
                $"Send CMD 663 MonsterAppearNtfList Count=1 NetId=0x{netId:X8} SpawnType=1 MonsterInfoId={BattleMonsterInfoId} EntGuid=0x{monster.EntGuid:X8} Faction=2 BTState=Idle Pos={FormatVec(spawnPos)}");

            client.SendCsProtoStructurePacket(monsterAppearList);

            CSBTObjSimpleLocomotion locomotion = new()
            {
                EntityId = netId,
                Position = new CSVec3(spawnPos.x, spawnPos.y, spawnPos.z),
                Rotation = monsterRot,
                TargetPos = new CSVec3(spawnPos.x, spawnPos.y, spawnPos.z),
            };

            Logger.Info(client,
                $"Send CMD 730 BTObjSimpleLocomotion EntityId=0x{netId:X8} Pos={FormatVec(locomotion.Position)} Target={FormatVec(locomotion.TargetPos)}");

            client.SendCsPacket(NewCsPacket.BTObjSimpleLocomotion(locomotion));

            client.State.PendingMonsterSpawnPos = null;
            client.State.PendingMonsterNetId = null;
            client.State.StartBattleMonsterLoop(netId, spawnPos);
            return;
        }

        Logger.Info(client,
            $"Receive CMD 534 without matching pending battle monster NetId=0x{pendingMonsterNetId:X8}; request IDs did not include the queued monster");
    }

    private static string FormatVec(CSVec3 vec)
    {
        return $"({vec.x:F3}, {vec.y:F3}, {vec.z:F3})";
    }
}
