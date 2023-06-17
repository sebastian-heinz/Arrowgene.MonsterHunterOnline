using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 解锁副本信息
/// </summary>
public struct PlayerUnlockLevelInfo : IStructure
{
    public PlayerUnlockLevelInfo()
    {
        LevelId = 0;
    }
    
    /// <summary>
    /// 解锁副本ID
    /// </summary>
    public int LevelId { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(LevelId, Endianness.Big);
    }
}