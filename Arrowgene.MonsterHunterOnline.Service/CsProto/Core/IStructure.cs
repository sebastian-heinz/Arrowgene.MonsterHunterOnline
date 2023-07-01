using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    public interface IStructure
    {
        void Write(IBuffer buffer);
        void Read(IBuffer buffer);
    }
}
