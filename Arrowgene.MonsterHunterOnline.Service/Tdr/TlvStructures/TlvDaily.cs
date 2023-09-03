using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+0x223720
/// </summary>
public class TlvDaily : Structure, ITlvStructure
{
    private const int MaxDailyCount = 0x40;

    public TlvDaily()
    {
        Daily = new List<short>();
        RefreshTime = 0;
        RefreshLevel = 0;
    }

    public List<short> Daily { get; set; }
    public uint RefreshTime { get; set; }
    public int RefreshLevel { get; set; }
    public int CompleteCount { get; set; }


    public void WriteTlv(IBuffer buffer)
    {
        // dailyCount %d
        int dailyCount = Math.Min(Daily.Count, MaxDailyCount);
        WriteTlvInt16(buffer, 1, (short)dailyCount);

        // daily
        if (dailyCount > 0)
        {
            WriteTlvInt32(buffer, 2, dailyCount * 2);
            for (int i = 0; i < dailyCount; i++)
            {
                WriteInt16(buffer, Daily[i]);
            }
        }

        // refreshTime %u
        WriteTlvUInt32(buffer, 3, RefreshTime);

        // refreshLevel %d
        WriteTlvInt32(buffer, 4, RefreshLevel);

        // completeCount %d
        WriteTlvInt32(buffer, 5, CompleteCount);
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}