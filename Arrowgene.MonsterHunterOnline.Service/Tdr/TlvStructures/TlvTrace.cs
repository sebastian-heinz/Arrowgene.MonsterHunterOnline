using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+0x222C10
/// </summary>
public class TlvTrace : Structure, ITlvStructure
{
    private const int MaxTraceCount = 0x10;
    private const int MaxTask = 0x10;

    public TlvTrace()
    {
        Time = 0;
        TraceCount = new List<int>();
        Task = new List<short>();
    }

    public List<int> TraceCount { get; }
    public List<short> Task { get; }
    public uint Time { get; set; }


    public void WriteTlv(IBuffer buffer)
    {
        // traceCount %d
        int resetCount = Math.Min(TraceCount.Count, MaxTraceCount);
        if (resetCount > 0)
        {
            WriteTlvInt32(buffer, 5, resetCount * 4);
            for (int i = 0; i < resetCount; i++)
            {
                WriteInt32(buffer, TraceCount[i]);
            }
        }
        
        // task
        int taskCount = Math.Min(Task.Count, MaxTask);
        if (taskCount > 0)
        {
            WriteTlvInt32(buffer, 4, taskCount * 2);
            for (int i = 0; i < taskCount; i++)
            {
                WriteInt16(buffer, Task[i]);
            }
        }

        // time %u
        WriteTlvUInt32(buffer, 3, Time);
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}