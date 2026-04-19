using System.Threading;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Protocol.Constant;
using Arrowgene.MonsterHunterOnline.Protocol.Old.Structures;
using Arrowgene.MonsterHunterOnline.Protocol.Structures;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.System;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class PlayerRegionJumpEndHandler : CsProtoStructureHandler<PlayerRegionJumpEnd>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(PlayerRegionJumpEnd));

    private static int _nextMonsterUniqueId;

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_PLAYER_REGION_JUMP_END;

    public override void Handle(Client client, PlayerRegionJumpEnd req)
    {
        if (client.State.PendingMonsterSpawnPos == null)
        {
            return;
        }

        if (client.State.PendingMonsterNetId != null)
        {
            Logger.Info(client, $"Skip duplicate battle monster queue for pending NetId=0x{client.State.PendingMonsterNetId.Value:X8}");
            return;
        }

        uint monsterNetId = NextMonsterNetId();
        client.State.PendingMonsterNetId = monsterNetId;

        CsCsProtoStructurePacket<EntityAppearNtfIdList> entityAppear = CsProtoResponse.EntityAppearNtfIdList;
        entityAppear.Structure.InitType = 1;
        entityAppear.Structure.LogicEntityId.Add(monsterNetId);
        entityAppear.Structure.LogicEntityType.Add((uint)LogicEntityType.MH_LETYPE_MONSTER);

        Logger.Info(client,
            $"Queue battle monster spawn NetId=0x{monsterNetId:X8} Type={(uint)LogicEntityType.MH_LETYPE_MONSTER} Pos={FormatVec(client.State.PendingMonsterSpawnPos)} via CMD 533");

        client.SendCsProtoStructurePacket(entityAppear);
    }

    private static uint NextMonsterNetId()
    {
        LogicEntityId entityId = new();
        entityId.Type = LogicEntityType.MH_LETYPE_MONSTER;
        entityId.UniqueId = (uint)Interlocked.Increment(ref _nextMonsterUniqueId);
        return entityId.Id;
    }

    private static string FormatVec(CSVec3 vec)
    {
        return $"({vec.x:F3}, {vec.y:F3}, {vec.z:F3})";
    }
}
