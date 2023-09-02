using System;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

public class TlvTask : Structure, ITlvStructure
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

    public void WriteTlv(IBuffer buffer)
    {
        WriteTlvInt16(buffer, 1, Id);
        WriteTlvByte(buffer, 2, State);
        WriteTlvInt32(buffer, 3, AcceptTime);
        WriteTlvInt32(buffer, 4, Timeout);
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}