using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdBattleActorMoveStateHandler : ICsProtoHandler
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CsCmdBattleActorMoveStateHandler));


    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_BATTLE_ACTOR_MOVESTATE;


    public void Handle(Client client, CsProtoPacket packet)
    {
        CSActorMovestate req = new CSActorMovestate();
        req.Read(packet.NewBuffer());

        //req.MoveSpeed.x = 10;
        Logger.Info($"NetObjId:{client.Character.Id} State:{req.State} Speed:X{req.MoveSpeed.x} Y{req.MoveSpeed.y} Z{req.MoveSpeed.z}");
        client.SendCsPacket(NewCsPacket.ActorMovestateNtf(new CSActorMovestateNtf()
        {
            NetObjId = client.Character.Id,
            ActorMovestate = req
        }));
        
        //client.SendCsPacket(NewCsPacket.ActorMovestate(req));
  
        
    }
}