using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 快捷栏数据
/// </summary>
public class CSShortcut : IStructure
{
    public CSShortcut()
    {
        Pos = 0;
        ItemId = 0;
    }

    /// <summary>
    /// 快捷栏位置
    /// </summary>
    public uint Pos { get; set; }

    /// <summary>
    /// 道具ID
    /// </summary>
    public uint ItemId { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteUInt32(Pos, Endianness.Big);
        buffer.WriteUInt32(ItemId, Endianness.Big);
    }
}