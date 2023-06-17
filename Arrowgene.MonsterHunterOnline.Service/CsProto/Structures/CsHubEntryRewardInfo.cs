using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// hub页签奖励
/// </summary>
public struct CsHubEntryRewardInfo : IStructure
{
    public CsHubEntryRewardInfo()
    {
        HubId = 0;
        PageIndex = 0;
    }
    
    /// <summary>
    /// 解锁副本ID
    /// </summary>
    public byte HubId { get; set; }
    
    /// <summary>
    /// 页签
    /// </summary>
    public int PageIndex { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteByte(HubId);
        buffer.WriteInt32(PageIndex, Endianness.Big);
    }
}