using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 星蕴石信息
/// </summary>
public class CSStarStoneInfo : IStructure
{
    public CSStarStoneInfo()
    {
        WaterExp = 0;
        FireExp = 0;
        ThunderExp = 0;
        DragonExp = 0;
        IceExp = 0;
        Duration = 0;
        Slots = new List<CSStarStoneSlotInfo>();
    }


    /// <summary>
    /// 水属性经验
    /// </summary>
    public int WaterExp { get; set; }

    /// <summary>
    /// 火属性经验
    /// </summary>
    public int FireExp { get; set; }

    /// <summary>
    /// 雷属性经验
    /// </summary>
    public int ThunderExp { get; set; }

    /// <summary>
    /// 龙属性经验
    /// </summary>
    public int DragonExp { get; set; }

    /// <summary>
    /// 冰属性经验
    /// </summary>
    public int IceExp { get; set; }

    /// <summary>
    /// 耐久度
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// 所有打开的SLOT的注入信息
    /// </summary>
    public List<CSStarStoneSlotInfo> Slots { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(WaterExp, Endianness.Big);
        buffer.WriteInt32(FireExp, Endianness.Big);
        buffer.WriteInt32(ThunderExp, Endianness.Big);
        buffer.WriteInt32(DragonExp, Endianness.Big);
        buffer.WriteInt32(IceExp, Endianness.Big);
        buffer.WriteInt32(Duration, Endianness.Big);

        int slotsSize = Slots.Count;
        buffer.WriteInt32(slotsSize, Endianness.Big);
        for (int i = 0; i < slotsSize; i++)
        {
            Slots[i].Write(buffer);
        }
    }
}