using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 副本综合信息
/// </summary>
public struct PlayerLevelIntegrateInfo : IStructure
{
    public PlayerLevelIntegrateInfo()
    {
        LevelId = 0;
        TheBestScore = 0;
        State = 0;
        HistoryFinalRank = 0;
        GainRewardFlag = 0;
        LastTm = 0;
    }

    /// <summary>
    /// 副本ID
    /// </summary>
    public int LevelId { get; set; }

    /// <summary>
    /// 历史最好成绩
    /// </summary>
    public int TheBestScore { get; set; }

    /// <summary>
    /// 历史状态，没有进去过，进去过但是没有过关，胜利过，失败
    /// </summary>
    public byte State { get; set; }

    /// <summary>
    /// 历史评价成绩，按位统计
    /// </summary>
    public uint HistoryFinalRank { get; set; }

    /// <summary>
    /// 历史成绩对应的领取与否的FLAG,按位统计
    /// </summary>
    public uint GainRewardFlag { get; set; }

    /// <summary>
    /// 最近时间
    /// </summary>
    public uint LastTm { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(LevelId, Endianness.Big);
        buffer.WriteInt32(TheBestScore, Endianness.Big);
        buffer.WriteByte(State);
        buffer.WriteUInt32(HistoryFinalRank, Endianness.Big);
        buffer.WriteUInt32(GainRewardFlag, Endianness.Big);
        buffer.WriteUInt32(LastTm, Endianness.Big);
    }
}