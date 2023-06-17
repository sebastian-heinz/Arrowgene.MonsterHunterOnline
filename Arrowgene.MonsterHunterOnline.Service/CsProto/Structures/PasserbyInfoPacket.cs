using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 路人信息数据
/// </summary>
public class PasserbyInfoPacket : IStructure
{
    public PasserbyInfoPacket()
    {
        RoleDbId = 0;
        Level = 0;
        RoleName = "";
        MeetWay = 0;
        HRLevel = 0;
        SvrId = 0;
    }


    public ulong RoleDbId { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    public int Level { get; set; }

    public string RoleName { get; set; }

    /// <summary>
    /// 结识途径
    /// </summary>
    public byte MeetWay { get; set; }

    /// <summary>
    /// HR等级
    /// </summary>
    public int HRLevel { get; set; }

    public int SvrId { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteUInt64(RoleDbId, Endianness.Big);
        buffer.WriteInt32(Level, Endianness.Big);
        buffer.WriteInt32(RoleName.Length + 1, Endianness.Big);
        buffer.WriteCString(RoleName);
        buffer.WriteByte(MeetWay);
        buffer.WriteInt32(HRLevel, Endianness.Big);
        buffer.WriteInt32(SvrId, Endianness.Big);
    }
}