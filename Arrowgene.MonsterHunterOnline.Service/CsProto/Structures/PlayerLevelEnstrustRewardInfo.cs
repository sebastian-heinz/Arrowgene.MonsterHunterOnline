using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 委托小组奖励领取记录
/// </summary>
public struct PlayerLevelEnstrustRewardInfo : IStructure
{

    public PlayerLevelEnstrustRewardInfo()
    {
        EnSubGroupId = 0;
        State = 0;
    }
    
    /// <summary>
    /// 小组ID
    /// </summary>
    public int EnSubGroupId { get; set; }

    /// <summary>
    /// 1表示已经领取过，0表示没有领取过
    /// </summary>
    public byte State { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(EnSubGroupId, Endianness.Big);
        buffer.WriteByte(State);
    }
}