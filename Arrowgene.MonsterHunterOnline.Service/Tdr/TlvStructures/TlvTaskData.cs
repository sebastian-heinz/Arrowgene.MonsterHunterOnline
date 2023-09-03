using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+0x225D00
/// </summary>
public class TlvTaskData : Structure, ITlvStructure
{
    private const int TasksMaxSize = 0x80;
    private const int MaxContentCount = 0x100;
    private const int MaxCompleteBitCount = 0x500;
    private const int MaxXDailyCount = 0x20;

    public TlvTaskData()
    {
        Tasks = new List<TlvTaskEntry>();
        Contents = new List<TlvContentEntry>();
        CompleteBit = new List<byte>();
        Daily = new TlvDaily();
        Schedule = new TlvSchedule();
        XDaily = new List<TlvXDaily>();
        Reset = new TlvReset();
        Trace = new TlvTrace();
        Complete = new TlvComplete();
    }

    public List<TlvTaskEntry> Tasks { get; }
    public List<TlvContentEntry> Contents { get; }
    public List<byte> CompleteBit { get; }
    public TlvDaily Daily { get; }
    public TlvSchedule Schedule { get; }
    public List<TlvXDaily> XDaily { get; }
    public TlvReset Reset { get; }
    public TlvTrace Trace { get; }
    public TlvComplete Complete { get; }

    public void WriteTlv(IBuffer buffer)
    {
        int taskCount = Math.Min(Tasks.Count, TasksMaxSize);

        // taskCount
        WriteTlvInt32(buffer, 1, taskCount);

        // task
        WriteTlvSubStructureList(buffer, 2, taskCount, Tasks);

        // contentCount

        int contentCount = Math.Min(Contents.Count, MaxContentCount);
        WriteTlvInt32(buffer, 3, contentCount);

        // content
        WriteTlvSubStructureList(buffer, 4, contentCount, Contents);

        // completeBitCount
        int completeBitCount = Math.Min(CompleteBit.Count, MaxCompleteBitCount);
        WriteTlvInt32(buffer, 5, completeBitCount);

        // completeBit
        if (completeBitCount > 0)
        {
            WriteTlvInt32(buffer, 6, completeBitCount * 1);
            for (int i = 0; i < completeBitCount; i++)
            {
                WriteByte(buffer, CompleteBit[i]);
            }
        }

        // daily
        WriteTlvSubStructure(buffer, 13, Daily);

        // schedule
        WriteTlvSubStructure(buffer, 14, Schedule);

        // xDailyCount
        int xDailyCount = Math.Min(XDaily.Count, MaxXDailyCount);
        WriteTlvInt32(buffer, 15, xDailyCount);

        // xDaily
        WriteTlvSubStructureList(buffer, 16, xDailyCount, XDaily);

        // reset
        WriteTlvSubStructure(buffer, 17, Reset);

        // trace
        WriteTlvSubStructure(buffer, 18, Trace);

        // complete
        WriteTlvSubStructure(buffer, 18, Complete);
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}