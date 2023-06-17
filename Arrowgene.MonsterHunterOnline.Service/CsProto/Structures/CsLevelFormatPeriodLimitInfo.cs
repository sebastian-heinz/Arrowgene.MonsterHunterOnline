using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 赛制周期限制信息
/// </summary>
public struct CsLevelFormatPeriodLimitInfo : IStructure
{
    public CsLevelFormatPeriodLimitInfo()
    {
        MergeId = 0;
        SubGroupId = 0;
        FinishCount = 0;
    }
    
    /// <summary>
    /// 合并的赛制
    /// </summary>
    public int MergeId { get; set; }
    
    /// <summary>
    /// SubGroupID
    /// </summary>
    public int SubGroupId { get; set; }
    
    /// <summary>
    /// 完成次数
    /// </summary>
    public int FinishCount { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(MergeId, Endianness.Big);
        buffer.WriteInt32(SubGroupId, Endianness.Big);
        buffer.WriteInt32(FinishCount, Endianness.Big);
    }
}