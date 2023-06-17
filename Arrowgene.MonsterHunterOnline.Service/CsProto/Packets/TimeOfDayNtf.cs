using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;

/// <summary>
/// TOD时间同步
/// </summary>
public class TimeOfDayNtf : CsPacket
{
    public override CsProtoCmd Cmd => CsProtoCmd.CS_CMD_TIME_OF_DAY_NTF;


    public TimeOfDayNtf()
    {
        Time = 0;
    }

    /// <summary>
    /// TOD时间同步
    /// </summary>
    public float Time { get; set; }

    public override void Write(IBuffer buffer)
    {
        buffer.WriteFloat(Time, Endianness.Big);
    }
}