using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 个人武器Style数据
/// </summary>
public class S2CWeaponStyleInfo : IStructure
{
    public S2CWeaponStyleInfo()
    {
        WeaponStyleData = new int[CsConstant.MAX_WEAPOSTYLE_TYPE];
    }

    /// <summary>
    /// 各类武器的Style
    /// </summary>
    public int[] WeaponStyleData { get; }


    public void Write(IBuffer buffer)
    {
        for (int i = 0; i < CsConstant.MAX_WEAPOSTYLE_TYPE; i++)
        {
            buffer.WriteInt32(WeaponStyleData[i], Endianness.Big);
        }
    }
}