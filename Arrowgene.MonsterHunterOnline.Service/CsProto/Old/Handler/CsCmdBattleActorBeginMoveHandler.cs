using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdBattleActorBeginMoveHandler : ICsProtoHandler
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CsCmdBattleActorBeginMoveHandler));


    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_BATTLE_ACTOR_BEGINMOVE;


    public void Handle(Client client, CsProtoPacket packet)
    {
        CSActorBeginmove req = new CSActorBeginmove();
        req.Read(packet.NewBuffer());
        
        
        Logger.Info($"BeginMove: Speed:X{req.MoveSpeed.x} Y{req.MoveSpeed.y} Z{req.MoveSpeed.z}");
        client.SendCsPacket(NewCsPacket.ActorBeginmoveNtf(new CSActorBeginmoveNtf()
        {
            NetObjId = client.Character.Id,
            ActorBeginmove = req
        }));
    }
}