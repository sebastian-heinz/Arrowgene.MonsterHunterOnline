using System;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+0x21F5F0
/// </summary>
public class TlvContentEntry : Structure, ITlvStructure
{
    public TlvContentEntry()
    {
        Task = 0;
        Id = 0;
        State = 0;
        Arg1 = 0;
    }

    public short Task { get; set; }
    public byte Id { get; set; }
    public byte State { get; set; }
    public byte Arg1 { get; set; }


    public void WriteTlv(IBuffer buffer)
    {
        // task %d
        WriteTlvInt16(buffer, 1, Task);

        // id %d
        WriteTlvByte(buffer, 2, Id);

        // state %d
        WriteTlvByte(buffer, 3, State);

        // arg1 %d
        WriteTlvByte(buffer, 4, Arg1);
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}