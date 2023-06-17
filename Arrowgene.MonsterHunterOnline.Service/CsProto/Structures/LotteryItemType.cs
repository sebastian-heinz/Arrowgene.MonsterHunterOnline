using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 机密研究院 奖励物品信息
/// </summary>
public class LotteryItemType : IStructure
{
    public LotteryItemType()
    {
        Position = 0;
        ItemId = 0;
        ItemNum = 0;
    }

    /// <summary>
    /// 奖品所在位置
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// 奖品ID
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// 奖品数量
    /// </summary>
    public int ItemNum { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(Position, Endianness.Big);
        buffer.WriteInt32(ItemId, Endianness.Big);
        buffer.WriteInt32(ItemNum, Endianness.Big);
    }
}