using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 机密研究院 抽奖盒子信息
/// </summary>
public class ItemBoxType : IStructure
{
    public ItemBoxType()
    {
        BoxId = 0;
        LotteryItemList = new LotteryItemType[CsConstant.SECRET_RESEARCH_LAB_LOTTERY_ITEM_MAX_LEN];
        for (int i = 0; i < CsConstant.SECRET_RESEARCH_LAB_LOTTERY_ITEM_MAX_LEN; i++)
        {
            LotteryItemList[i] = new LotteryItemType();
        }
        VipRefrshCount = 0;
        RefreshCount = 0;
        ResearchCount = 0;
    }

    /// <summary>
    /// 盒子ID
    /// </summary>
    public int BoxId { get; set; }

    /// <summary>
    /// 奖励槽信息
    /// </summary>
    public LotteryItemType[] LotteryItemList { get; set; }

    /// <summary>
    /// VIP刷新次数
    /// </summary>
    public int VipRefrshCount { get; set; }

    /// <summary>
    /// 刷新次数
    /// </summary>
    public int RefreshCount { get; set; }

    /// <summary>
    /// 研究次数
    /// </summary>
    public int ResearchCount { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(BoxId, Endianness.Big);
        for (int i = 0; i < CsConstant.SECRET_RESEARCH_LAB_LOTTERY_ITEM_MAX_LEN; i++)
        {
            LotteryItemList[i].Write(buffer);
        }

        buffer.WriteInt32(VipRefrshCount, Endianness.Big);
        buffer.WriteInt32(RefreshCount, Endianness.Big);
        buffer.WriteInt32(ResearchCount, Endianness.Big);
    }
}