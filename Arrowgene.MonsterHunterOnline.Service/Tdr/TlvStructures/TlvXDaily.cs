using System;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+0x224580
/// </summary>
public class TlvXDaily : Structure, ITlvStructure
{
    public TlvXDaily()
    {
        RefreshTime = 0;
        Lib = 0;
        CompleteCount = 0;
        RemainCount = 0;
    }

    public int RefreshTime { get; set; }
    public int Lib { get; set; }
    public int CompleteCount { get; set; }
    public int RemainCount { get; set; }


    public void WriteTlv(IBuffer buffer)
    {
        // refreshTime %u
        WriteTlvInt32(buffer, 1, RefreshTime);
        // lib %d
        WriteTlvInt32(buffer, 2, Lib);
        // completeCount %d
        WriteTlvInt32(buffer, 3, CompleteCount);
        // remainCount %d
        WriteTlvInt32(buffer, 4, RemainCount);
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}