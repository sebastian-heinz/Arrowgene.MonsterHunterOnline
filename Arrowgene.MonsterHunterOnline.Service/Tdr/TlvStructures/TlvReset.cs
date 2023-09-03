using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+221A20
/// </summary>
public class TlvReset : Structure, ITlvStructure
{
    private const int MaxResetCount = 0x40;
    private const int MaxTaskCount = 0x40;

    public TlvReset()
    {
        Time = 0;
        Task = new List<short>();
        ResetCount = new List<int>();
    }

    public uint Time { get; set; }
    public List<short> Task { get; }
    public List<int> ResetCount { get; }

    public void WriteTlv(IBuffer buffer)
    {
        // resetCount %d
        int resetCount = Math.Min(ResetCount.Count, MaxResetCount);
        if (resetCount > 0)
        {
            WriteTlvInt32(buffer, 5, resetCount * 4);
            for (int i = 0; i < resetCount; i++)
            {
                WriteInt32(buffer, ResetCount[i]);
            }
        }


        // task
        int taskCount = Math.Min(Task.Count, MaxTaskCount);
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