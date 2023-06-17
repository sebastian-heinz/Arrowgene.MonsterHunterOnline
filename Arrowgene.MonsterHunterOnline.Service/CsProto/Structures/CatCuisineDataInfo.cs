using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 猫饭解锁信息
/// </summary>
public class CatCuisineDataInfo : IStructure
{
    public CatCuisineDataInfo()
    {
        CatCuisineId = 0;
        State = 0;
    }

    /// <summary>
    /// 配方ID
    /// </summary>
    public int CatCuisineId { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public uint State { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(CatCuisineId, Endianness.Big);
        buffer.WriteUInt32(State, Endianness.Big);
    }
}