using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    public interface ICsStructure
    {
        void WriteCs(IBuffer buffer);
        void ReadCs(IBuffer buffer);
    }
}
