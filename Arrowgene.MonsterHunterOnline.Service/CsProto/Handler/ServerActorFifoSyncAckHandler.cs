using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class ServerActorFifoSyncAckHandler : CsProtoStructureHandler<ServerSyncInfoAck>
{
    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_SERVER_ACTOR_FIFO_SYNC_ACK;


    public override void Handle(Client client, ServerSyncInfoAck req)
    {
    }
}