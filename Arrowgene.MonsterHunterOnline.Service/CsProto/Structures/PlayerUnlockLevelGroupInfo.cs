using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 解锁关卡组信息
/// </summary>
public struct PlayerUnlockLevelGroupInfo : IStructure
{
    public PlayerUnlockLevelGroupInfo()
    {
        LevelGroupId = 0;
    }
    
    /// <summary>
    /// 解锁关卡组ID
    /// </summary>
    public int LevelGroupId { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(LevelGroupId, Endianness.Big);
    }
}