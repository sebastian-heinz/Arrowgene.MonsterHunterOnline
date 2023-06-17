using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class CSVec3 : IStructure
{
    public CSVec3()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteFloat(X, Endianness.Big);
        buffer.WriteFloat(Y, Endianness.Big);
        buffer.WriteFloat(Z, Endianness.Big);
    }
}