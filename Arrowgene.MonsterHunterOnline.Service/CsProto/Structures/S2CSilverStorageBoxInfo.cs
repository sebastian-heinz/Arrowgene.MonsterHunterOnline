using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 银币收纳箱
/// </summary>
public class S2CSilverStorageBoxInfo : IStructure
{
    public S2CSilverStorageBoxInfo()
    {
        SilverCount = 0;
        WeekFreeFetchTimes = 0;
        WeekBuyFetchTimes = 0;
        EnlargeTimes = 0;
    }

    /// <summary>
    /// 银币收纳箱中的当前银币量
    /// </summary>
    public int SilverCount { get; set; }

    /// <summary>
    /// 银币收纳箱本周已免费提取银币次数
    /// </summary>
    public int WeekFreeFetchTimes { get; set; }

    /// <summary>
    /// 银币收纳箱本周已付费提取银币次数
    /// </summary>
    public int WeekBuyFetchTimes { get; set; }

    /// <summary>
    /// 累计扩容次数
    /// </summary>
    public int EnlargeTimes { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(SilverCount, Endianness.Big);
        buffer.WriteInt32(WeekFreeFetchTimes, Endianness.Big);
        buffer.WriteInt32(WeekBuyFetchTimes, Endianness.Big);
        buffer.WriteInt32(EnlargeTimes, Endianness.Big);
    }
}