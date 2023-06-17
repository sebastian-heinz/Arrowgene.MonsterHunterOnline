using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class CSQuat : IStructure
{
    public CSQuat()
    {
        V = new CSVec3();
        W = 0;
    }

    public CSVec3 V { get; set; }
    public float W { get; set; }

    public void Write(IBuffer buffer)
    {
        V.Write(buffer);
        buffer.WriteFloat(W, Endianness.Big);
    }
}