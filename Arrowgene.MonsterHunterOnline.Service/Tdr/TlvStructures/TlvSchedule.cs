using System;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+0x223EE0
/// </summary>
public class TlvSchedule : Structure, ITlvStructure
{
    public TlvSchedule()
    {
        RefreshTime = 0;
    }

    public uint RefreshTime { get; set; }

    public void WriteTlv(IBuffer buffer)
    {
        // refreshTime %u
        WriteTlvUInt32(buffer, 1, RefreshTime);
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}