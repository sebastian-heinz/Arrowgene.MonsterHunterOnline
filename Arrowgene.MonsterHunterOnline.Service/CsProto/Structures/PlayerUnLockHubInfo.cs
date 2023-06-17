using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 解锁页签类型
/// </summary>
public struct PlayerUnLockHubInfo : IStructure
{

    public PlayerUnLockHubInfo()
    {
        HubId = 0;
        PageIndex = 0;
    }
    
    /// <summary>
    /// HubID
    /// </summary>
    public int HubId { get; set; }

    /// <summary>
    /// 页签ID
    /// </summary>
    public int PageIndex { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(HubId, Endianness.Big);
        buffer.WriteInt32(PageIndex, Endianness.Big);
    }
}