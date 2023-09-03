using System;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+0x177AE0
/// </summary>
public class TlvRapidHunt : Structure, ITlvStructure
{
    public TlvRapidHunt()
    {
        RefreshTime = 0;
    }

    public uint RefreshTime { get; set; }

    public void WriteTlv(IBuffer buffer)
    {
        // refreshTime %u
        WriteTlvUInt32(buffer, 1, RefreshTime);

        // count
        // awardsType
        // awardsState
        // awardsId
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}