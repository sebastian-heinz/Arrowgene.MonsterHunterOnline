using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdSystemPkgTimerRecordHandler : ICsProtoHandler
{
    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_SYSTEM_PKG_TIMER_RECORD;

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
    }
}