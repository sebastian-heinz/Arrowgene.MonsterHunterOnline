using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 星蕴石注入信息
/// </summary>
public class CSStarStoneSlotInfo : IStructure
{
    public CSStarStoneSlotInfo()
    {
        ItemId = 0;
        Count = 0;
    }

    /// <summary>
    /// 注入的道具
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// 注入的数量
    /// </summary>
    public int Count { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(ItemId, Endianness.Big);
        buffer.WriteInt32(Count, Endianness.Big);
    }
}