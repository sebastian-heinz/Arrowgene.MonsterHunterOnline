using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

public class TlvEmpty : Structure, ITlvStructure
{
    public TlvEmpty()
    {
    }

    public void WriteTlv(IBuffer buffer)
    {
    }

    public void ReadTlv(IBuffer buffer)
    {
    }
}