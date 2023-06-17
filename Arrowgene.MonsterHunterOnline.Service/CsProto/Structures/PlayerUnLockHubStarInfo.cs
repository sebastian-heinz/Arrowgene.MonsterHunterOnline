using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 解锁星级类型
/// </summary>
public struct PlayerUnLockHubStarInfo : IStructure
{
    public PlayerUnLockHubStarInfo()
    {
        HubId = 0;
        PageIndex = 0;
        StarLv = 0;
    }
    
    /// <summary>
    /// HubID
    /// </summary>
    public int HubId { get; set; }

    /// <summary>
    /// 页签ID
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 星级等级
    /// </summary>
    public int StarLv { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(HubId, Endianness.Big);
        buffer.WriteInt32(PageIndex, Endianness.Big);
        buffer.WriteInt32(StarLv, Endianness.Big);
    }
}