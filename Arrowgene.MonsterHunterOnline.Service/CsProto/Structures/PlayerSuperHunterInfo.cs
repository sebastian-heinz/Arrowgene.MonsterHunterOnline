using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 超级大连续信息
/// </summary>
public struct PlayerSuperHunterInfo : IStructure
{
    public PlayerSuperHunterInfo()
    {
        LastResetTime = 0;
        GainChallangeRewardTimes = 0;
        GainSuccessRewardTimes = 0;
        GainVipChallangeRewardTimes = 0;
        GainVipSuccessRewardTimes = 0;
    }

    /// <summary>
    /// 上次重置挑战信息时间
    /// </summary>
    public uint LastResetTime { get; set; }

    /// <summary>
    /// 今日领取挑战奖励次数
    /// </summary>
    public int GainChallangeRewardTimes { get; set; }

    /// <summary>
    /// 今日领取成功奖励次数
    /// </summary>
    public int GainSuccessRewardTimes { get; set; }

    /// <summary>
    /// 今日领取Vip挑战奖励次数
    /// </summary>
    public int GainVipChallangeRewardTimes { get; set; }

    /// <summary>
    /// 今日领取Vip成功奖励次数
    /// </summary>
    public int GainVipSuccessRewardTimes { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteUInt32(LastResetTime, Endianness.Big);
        buffer.WriteInt32(GainChallangeRewardTimes, Endianness.Big);
        buffer.WriteInt32(GainSuccessRewardTimes, Endianness.Big);
        buffer.WriteInt32(GainVipChallangeRewardTimes, Endianness.Big);
        buffer.WriteInt32(GainVipSuccessRewardTimes, Endianness.Big);
    }
}