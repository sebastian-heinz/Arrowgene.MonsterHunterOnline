using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class BattleActorBeginMoveHandler : CsProtoStructureHandler<ActorBeginMove>
{
    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_BATTLE_ACTOR_BEGINMOVE;


    public override void Handle(Client client, ActorBeginMove req)
    {
        CsCsProtoStructurePacket<ActorBeginMoveNtf> actorBeginMoveNtf = CsProtoResponse.ActorBeginMoveNtf;
        actorBeginMoveNtf.Structure.NetObjId = client.Character.Id;
        actorBeginMoveNtf.Structure.ActorBeginMove = req;
       // client.SendCsProtoStructurePacket(actorBeginMoveNtf);
    }
}