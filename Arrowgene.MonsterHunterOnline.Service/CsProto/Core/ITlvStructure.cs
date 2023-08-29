using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    public interface ITlvStructure
    {
        void WriteTlv(IBuffer buffer);
        void ReadTlv(IBuffer buffer);
    }
}
