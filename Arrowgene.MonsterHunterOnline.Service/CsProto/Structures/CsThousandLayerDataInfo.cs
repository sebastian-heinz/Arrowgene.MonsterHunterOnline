using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 千层塔每层通关时间
/// </summary>
public struct CsThousandLayerDataInfo : IStructure
{
    public CsThousandLayerDataInfo()
    {
        LevelId = 0;
        Seconds = 0;
        Layer = 0;
    }
    
    /// <summary>
    /// 关卡ID
    /// </summary>
    public int LevelId { get; set; }

    /// <summary>
    /// 每层最好成绩
    /// </summary>
    public int Seconds { get; set; }

    /// <summary>
    /// 层数
    /// </summary>
    public short Layer { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(LevelId, Endianness.Big);
        buffer.WriteInt32(Seconds, Endianness.Big);
        buffer.WriteInt16(Layer, Endianness.Big);
    }
}