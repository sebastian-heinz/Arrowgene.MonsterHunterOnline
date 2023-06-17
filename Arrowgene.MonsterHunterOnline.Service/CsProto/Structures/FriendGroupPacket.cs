using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 好友分组信息数据
/// </summary>
public class FriendGroupPacket : IStructure
{
    public FriendGroupPacket()
    {
        GroupId = 0;
        GroupName = "";
    }


    public byte GroupId { get; set; }

    public string GroupName { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteByte(GroupId);
        buffer.WriteInt32(GroupName.Length + 1, Endianness.Big);
        buffer.WriteCString(GroupName);
    }
}