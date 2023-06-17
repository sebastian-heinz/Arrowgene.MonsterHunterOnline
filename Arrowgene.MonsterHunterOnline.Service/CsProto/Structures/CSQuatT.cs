using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class CSQuatT : IStructure
{
    public CSQuatT()
    {
        Q = new CSQuat();
        T = new CSVec3();
    }
    
    public CSQuat Q { get; set; }
    public CSVec3 T { get; set; }

    public void Write(IBuffer buffer)
    {
        Q.Write(buffer);
        T.Write(buffer);
    }
}