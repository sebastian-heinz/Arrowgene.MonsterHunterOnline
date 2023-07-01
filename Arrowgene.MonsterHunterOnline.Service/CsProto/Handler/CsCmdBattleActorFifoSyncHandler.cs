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
        IBuffer req = new StreamBuffer(packet.Body);
        req.SetPositionStart();
        CSFIFOSyncInfo info = new CSFIFOSyncInfo();
        info.Read(req);

        //  <struct name="CSAGSyncInfo" version="1" desc="AG同步信息">
        //      <entry name="EntityId" type="uint32" desc="需要同步的Actor的EntityId"/>
        //      <entry name="Layer" type="uint8" desc="AGLayerID"/>
        //      <entry name="InputType" type="uint8" desc="AGInput类型，0－int，1－float"/>
        //      <entry name="Input" type="uint8" desc="AGInput"/>
        //      <entry name="Value" type="uint32" desc="AGInput取值"/>
        //      <entry name="StateID" type="uint32" desc="当前State的ID"/>
        //      <entry name="ratio" type="float" desc="当前State的ratio"/>
        //      </struct>

        //CS_CMD_SERVER_ACTOR_FIFO_SYNC_NTF
        // client.SendCsPacket(NewCsPacket.FIFOSyncMsg(new CSFIFOSyncInfo()));
        //  client.SendCsPacket(NewCsPacket.FIFOSyncMsgNtf(new CSFIFOSyncInfoNtf()
        //  {
        //      EntityId = 0,
        //      SyncInfo = info
        //  }));
        client.SendCsPacket(NewCsPacket.ServerSyncInfoAck(new CSServerSyncInfoAck()
        {
            SyncTime = info.SyncTime
        }));
    }
}