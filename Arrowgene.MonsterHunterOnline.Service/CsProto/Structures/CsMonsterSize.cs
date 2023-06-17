using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 历史怪物尺寸
/// </summary>
public struct CsMonsterSize : IStructure
{
    public CsMonsterSize()
    {
        MonsterId = 0;
        MinSize = 0;
        MaxSize = 0;
        MaxFlag = 0;
        MinFlag = 0;
    }
    
    /// <summary>
    /// 怪物ID
    /// </summary>
    public int MonsterId { get; set; }

    /// <summary>
    /// 最小尺寸
    /// </summary>
    public float MinSize { get; set; }

    /// <summary>
    /// 最大尺寸
    /// </summary>
    public float MaxSize { get; set; }

    /// <summary>
    /// 记录标记Max
    /// </summary>
    public int MaxFlag { get; set; }

    /// <summary>
    /// 记录标记Min
    /// </summary>
    public int MinFlag { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(MonsterId, Endianness.Big);
        buffer.WriteFloat(MinSize, Endianness.Big);
        buffer.WriteFloat(MaxSize, Endianness.Big);
        buffer.WriteInt32(MaxFlag, Endianness.Big);
        buffer.WriteInt32(MinFlag, Endianness.Big);
    }
}