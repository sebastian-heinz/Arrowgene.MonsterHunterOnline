using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 装备强化数据
/// </summary>
public class CsItemRebuildLimitInfo : IStructure
{
    public CsItemRebuildLimitInfo()
    {
        LastRebuildTm = 0;
        RebuildLimitDataInfo = new List<CsItemRebuildLimitData>();
    }

    /// <summary>
    /// 最近强化优惠时间
    /// </summary>
    public ulong LastRebuildTm { get; set; }

    public List<CsItemRebuildLimitData> RebuildLimitDataInfo { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteUInt64(LastRebuildTm);
        byte rebuildLimitDataInfoSize = (byte)RebuildLimitDataInfo.Count;
        buffer.WriteByte(rebuildLimitDataInfoSize);
        for (int i = 0; i < rebuildLimitDataInfoSize; i++)
        {
            RebuildLimitDataInfo[i].Write(buffer);
        }
    }
}