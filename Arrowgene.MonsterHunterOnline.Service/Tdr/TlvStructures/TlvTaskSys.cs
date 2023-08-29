using System;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

public class TlvTaskSys : Structure, ITlvStructure
{
    public TlvTaskSys()
    {
    }
    
    public void WriteTlv(IBuffer buffer)
    {
        WriteByte(buffer, (byte)TlvMagic.NoVariant);
        int startPos = buffer.Position;
        WriteInt32(buffer, 0);


        WriteTlvInt32(buffer, 1, 1);
        WriteInt32(buffer, 0xABCDEF);


        int endPos = buffer.Position;
        int size = endPos - startPos + 1;
        buffer.Position = startPos;
        WriteInt32(buffer, size);
        buffer.Position = endPos;
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}