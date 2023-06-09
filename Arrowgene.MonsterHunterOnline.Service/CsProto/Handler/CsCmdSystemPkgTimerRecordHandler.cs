using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdSystemPkgTimerRecordHandler : ICsProtoHandler
{
    public CsProtoCmd Cmd => CsProtoCmd.CS_CMD_SYSTEM_PKG_TIMER_RECORD;

    public void Handle(Client client, CsProtoPacket packet)
    {
      //  <struct name="CSPkgTimerRecord" version="1" desc="协议包跟踪">
      //      <entry name="Timer1" type="bigint" desc="时间记录1"/>
      //      <entry name="Timer2" type="bigint" desc="时间记录1"/>
      //      <entry name="Timer3" type="bigint" desc="时间记录1"/>
      //      <entry name="Timer4" type="bigint" desc="时间记录1"/>
      //      <entry name="Timer5" type="bigint" desc="时间记录1"/>
      //      <entry name="Timer6" type="bigint" desc="时间记录1"/>
      //      <entry name="Timer7" type="bigint" desc="时间记录1"/>
      //      <entry name="Timer8" type="bigint" desc="时间记录1"/>
      //      <entry name="Timer9" type="bigint" desc="时间记录1"/>
      //      <entry name="Timer10" type="bigint" desc="时间记录1"/>
      //      </struct>
      
      
    // <struct name="CSPkgTransAntiData" version="1" desc="传输ANTI数据">
    //     <entry name="AntiDataLen" type="int32" desc="AntiData长度"/>
    //     <entry name="AntiData" type="byte" count="MAX_NORMAL_PKG_LENGTH" desc="AntiData的BUFF" refer="AntiDataLen"/>
    //     </struct>
    IBuffer res = new StreamBuffer();
    res.WriteInt32(26, Endianness.Big);
    res.WriteBytes(new byte[26]);
    
    CsProtoPacket resp = new CsProtoPacket();
    resp.Body = res.GetAllBytes();
    resp.Cmd = CsProtoCmd.CS_CMD_SYSTEM_TRANS_ANTI_DATA;
    client.SendCsProto(resp);
    }
}