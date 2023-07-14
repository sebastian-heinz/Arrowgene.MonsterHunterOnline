using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.Tdr;
using Xunit;

namespace Arrowgene.MonsterHunterOnline.Test.Service.Tdr;

public class TdrBufferTest
{
    [Fact]
    public void TestWriteVarUInt32()
    {
        StreamBuffer ast = new StreamBuffer();
        ast.WriteByte((byte)TdrTlvMagic.NoVariant);

        int sizePos = ast.Position;
        ast.WriteInt32(0, Endianness.Big); // size

        for (int i = 3; i < 10; i++)
        {
            uint tag = TdrTlv.MakeTag(i, TdrTlvType.ID_4_BYTE);
            TdrBuffer.WriteVarUInt32(ast, 0xFFFFFFFF);
            ast.WriteInt32(20, Endianness.Big); //attr val 
        }
    }
}