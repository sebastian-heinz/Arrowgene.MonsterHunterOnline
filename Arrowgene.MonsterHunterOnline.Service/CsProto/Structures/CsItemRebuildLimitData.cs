using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 装备强化次数数据
/// </summary>
public struct CsItemRebuildLimitData : IStructure
{
    public CsItemRebuildLimitData()
    {
        LimitType = 0;
        LimitCnt = 0;
    }

    /// <summary>
    /// 类型
    /// </summary>
    public int LimitType { get; set; }

    /// <summary>
    /// 次数
    /// </summary>
    public ushort LimitCnt { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(LimitType, Endianness.Big);
        buffer.WriteUInt16(LimitCnt, Endianness.Big);
    }
}