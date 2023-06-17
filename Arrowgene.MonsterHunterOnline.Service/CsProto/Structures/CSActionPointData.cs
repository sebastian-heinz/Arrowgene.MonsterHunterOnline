using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 行动同步数据
/// </summary>
public class CSActionPointData : IStructure
{
    public CSActionPointData()
    {
        ActionPoint = new int[CsConstant.ACTION_POINT_TYPE_COUNT];
        AdditionalActionPoint = 0;
        NextResetTime = 0;
        ActionPointFlags = 0;
    }

    /// <summary>
    /// 普通行动力
    /// </summary>
    public int[] ActionPoint { get; }

    /// <summary>
    /// 附加行动力
    /// </summary>
    public int AdditionalActionPoint { get; set; }

    /// <summary>
    /// 下次重置时间
    /// </summary>
    public int NextResetTime { get; set; }

    /// <summary>
    /// 行动力各项标签
    /// </summary>
    public uint ActionPointFlags { get; set; }


    public void Write(IBuffer buffer)
    {
        for (int i = 0; i < CsConstant.ACTION_POINT_TYPE_COUNT; i++)
        {
            buffer.WriteInt32(ActionPoint[i], Endianness.Big);
        }

        buffer.WriteInt32(AdditionalActionPoint, Endianness.Big);
        buffer.WriteInt32(NextResetTime, Endianness.Big);
        buffer.WriteUInt32(ActionPointFlags, Endianness.Big);
    }
}