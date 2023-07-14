using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class BattleActorFifoSyncHandler : CsProtoStructureHandler<FifoSyncInfo>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(BattleActorFifoSyncHandler));
    
    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_BATTLE_ACTOR_FIFO_SYNC;


    public override void Handle(Client client, FifoSyncInfo req)
    {
        // i think this is request for syncing stuff
        // client sends -> CS_CMD_BATTLE_ACTOR_FIFO_SYNC
        // looks like the best answer should be -> CS_CMD_BATTLE_ACTOR_FIFO_SYNC_NTF
        // question is do we send it to the player back as well
        
        // i think this flow is for forced updates by server, where client acknowledged
        // if server responds -> CS_CMD_SERVER_ACTOR_FIFO_SYNC_NTF
        // then client sends -> CS_CMD_SERVER_ACTOR_FIFO_SYNC_ACK
        
        CsProtoStructurePacket<ServerSyncInfoNtf> serverSyncInfoNtf = CsProtoResponse.ServerSyncInfoNtf;
        serverSyncInfoNtf.Structure.EntityId = client.Character.Id;
        serverSyncInfoNtf.Structure.SyncInfo = req;
        //client.SendCsProtoStructurePacket(serverSyncInfoNtf);
        
        CsProtoStructurePacket<FifoSyncInfoNtf> fifoSyncInfoNtf = CsProtoResponse.FifoSyncInfoNtf;
        fifoSyncInfoNtf.Structure.EntityId = client.Character.Id;
        fifoSyncInfoNtf.Structure.SyncInfo = req;
        // client.SendCsProtoStructurePacket(fifoSyncInfoNtf);
    }
}