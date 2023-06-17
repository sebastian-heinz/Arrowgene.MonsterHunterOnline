using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public interface IStructure
{
    void Write(IBuffer buffer);
}