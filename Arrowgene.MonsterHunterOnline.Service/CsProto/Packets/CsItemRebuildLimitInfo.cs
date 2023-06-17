using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;

/// <summary>
/// 装备强化数据
/// </summary>
public class CsItemRebuildLimitInfo : CsPacket
{
    public override CsProtoCmd Cmd => CsProtoCmd.CS_CMD_ITEMREBUILD_LIMITDATA_NTF;

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

    public override void Write(IBuffer buffer)
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