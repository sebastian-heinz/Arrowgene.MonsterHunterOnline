using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdBattleActorIdleMoveHandler : ICsProtoHandler
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CsCmdBattleActorIdleMoveHandler));


    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_BATTLE_ACTOR_IDLEMOVE;


    public void Handle(Client client, CsProtoPacket packet)
    {
        CSActorIdlemove req = new CSActorIdlemove();
        req.Read(packet.NewBuffer());
        
        client.SendCsPacket(NewCsPacket.ActorIdlemoveNtf(new CSActorIdlemoveNtf()
        {
            NetObjId  = client.Character.Id,
            ActorIdlemove = req
        }));
    }
}