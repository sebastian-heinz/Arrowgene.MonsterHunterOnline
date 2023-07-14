using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class BattleActorMoveStateHandler : CsProtoStructureHandler<ActorMoveState>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CreateRoleReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_BATTLE_ACTOR_MOVESTATE;


    public override void Handle(Client client, ActorMoveState req)
    {
        CsProtoStructurePacket<ActorMoveStateNtf> actorMoveStateNtf = CsProtoResponse.ActorMoveStateNtf;
        actorMoveStateNtf.Structure.NetObjId = client.Character.Id;
        actorMoveStateNtf.Structure.ActorMoveState = req;
       // client.SendCsProtoStructurePacket(actorMoveStateNtf);
    }
}