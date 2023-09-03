using System;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+0x21EDB0
/// </summary>
public class TlvTaskEntry : Structure, ITlvStructure
{
    public TlvTaskEntry()
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
        // id %d
        WriteTlvInt16(buffer, 1, Id);
        // state %d
        WriteTlvByte(buffer, 2, State);
        // acceptTime %d
        WriteTlvInt32(buffer, 3, AcceptTime);
        // timeout %d
        WriteTlvInt32(buffer, 4, Timeout);
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}