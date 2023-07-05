using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdBattleActorFifoSyncHandler : ICsProtoHandler
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CsCmdBattleActorFifoSyncHandler));


    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_BATTLE_ACTOR_FIFO_SYNC;


    public void Handle(Client client, CsProtoPacket packet)
    {
        CSFIFOSyncInfo req = new CSFIFOSyncInfo();
        req.Read(packet.NewBuffer());
        
        Logger.Info($"FifoSync: X{req.Pos.x} Y{req.Pos.y} Z{req.Pos.z} sp:{req.sp}");

        client.SendCsPacket(NewCsPacket.ServerSyncInfoAck(new CSServerSyncInfoAck()
        {
            SyncTime = req.SyncTime
        }));
    }
}