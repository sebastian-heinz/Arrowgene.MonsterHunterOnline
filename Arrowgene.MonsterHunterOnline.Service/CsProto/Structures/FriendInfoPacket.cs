using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 好友信息数据
/// </summary>
public class FriendInfoPacket : IStructure
{
    public FriendInfoPacket()
    {
        RoleDbId = 0;
        Level = 0;
        RoleName = "";
        GroupId = 0;
        Friendly = 0;
        FarmPoint = 0;
        FarmCanBeGatheredCount = 0;
        HRLevel = 0;
        SvrId = 0;
        AddTime = 0;
    }


    public ulong RoleDbId { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    public int Level { get; set; }

    public string RoleName { get; set; }

    /// <summary>
    /// 分组ID
    /// </summary>
    public byte GroupId { get; set; }

    /// <summary>
    /// 友好度
    /// </summary>
    public uint Friendly { get; set; }

    /// <summary>
    /// 农场点数
    /// </summary>
    public int FarmPoint { get; set; }

    /// <summary>
    /// 玩家农场可被采集的剩余次数
    /// </summary>
    public int FarmCanBeGatheredCount { get; set; }

    /// <summary>
    /// HR等级
    /// </summary>
    public int HRLevel { get; set; }

    public uint SvrId { get; set; }
    public int AddTime { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteUInt64(RoleDbId, Endianness.Big);
        buffer.WriteInt32(Level, Endianness.Big);
        buffer.WriteInt32(RoleName.Length + 1, Endianness.Big);
        buffer.WriteCString(RoleName);
        buffer.WriteByte(GroupId);
        buffer.WriteUInt32(Friendly, Endianness.Big);
        buffer.WriteInt32(FarmPoint, Endianness.Big);
        buffer.WriteInt32(FarmCanBeGatheredCount, Endianness.Big);
        buffer.WriteInt32(HRLevel, Endianness.Big);
        buffer.WriteUInt32(SvrId, Endianness.Big);
        buffer.WriteInt32(AddTime, Endianness.Big);
    }
}