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
        client.SendCsPacket(NewCsPacket.ActorMovestateNtf(new CSActorMovestateNtf()
        {
            NetObjId = client.State._spawnPlayer.NetObjId,
            ActorMovestate = req
        }));
    }
}