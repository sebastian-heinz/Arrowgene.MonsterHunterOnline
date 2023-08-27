using System;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

public class TlvTask : TlvStructure
{
    public TlvTask()
    {
        Id = 0;
        State = 0;
        AcceptTime = 0;
        Timeout = 0;
    }

    public short Id { get; set; }
    public byte State { get; set; }
    public int AcceptTime { get; set; }
    public int Timeout { get; set; }

    public override void Write(IBuffer buffer)
    {
        int startPos = buffer.Position;
        WriteInt32(buffer, 0);

        WriteTlvInt16(buffer, 1, Id);
        WriteTlvByte(buffer, 2, State);
        WriteTlvInt32(buffer, 3, AcceptTime);
        WriteTlvInt32(buffer, 4, Timeout);

        int endPos = buffer.Position;
        int entrySize = endPos - startPos - 4;
        buffer.Position = startPos;
        WriteInt32(buffer, entrySize);
        buffer.Position = endPos;
    }

    public override void Read(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}