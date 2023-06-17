using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 委托关卡信息
/// </summary>
public struct PlayerLevelEntrustInfo : IStructure
{
    public PlayerLevelEntrustInfo()
    {
        MergeGroupId = 0;
        LevelId = 0;
        NpcId = 0;
    }

    /// <summary>
    /// Merge GroupID
    /// </summary>
    public uint MergeGroupId { get; set; }

    /// <summary>
    /// 副本ID
    /// </summary>
    public int LevelId { get; set; }

    /// <summary>
    /// 接取的NPCID
    /// </summary>
    public int NpcId { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteUInt32(MergeGroupId, Endianness.Big);
        buffer.WriteInt32(LevelId, Endianness.Big);
        buffer.WriteInt32(NpcId, Endianness.Big);
    }
}