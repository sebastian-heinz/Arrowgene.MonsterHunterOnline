using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class MultiIspSequenceNtf : Structure
{
    /// <summary>
    /// 双线机房运营商顺序
    /// </summary>
    public MultiIspSequenceNtf()
    {
        Sequence = 0;
    }

    /// <summary>
    /// 双线机房运营商顺序
    /// </summary>
    public int Sequence { get; set; }


    public override void Write(IBuffer buffer)
    {
        WriteInt32(buffer, Sequence);
    }

    public override void Read(IBuffer buffer)
    {
        Sequence = ReadInt32(buffer);
    }
}