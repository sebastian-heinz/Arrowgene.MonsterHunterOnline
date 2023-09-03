using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+2207E0
/// </summary>
public class TlvComplete : Structure, ITlvStructure
{
    private const int MaxCompleteCount = 0x800;
    private const int MaxTask = 0x800;


    public TlvComplete()
    {
        CompleteCount = new List<byte>();
        Task = new List<short>();
        Count = 0;
    }

    public List<byte> CompleteCount { get; }
    public List<short> Task { get; }
    public int Count { get; set; }

    public void WriteTlv(IBuffer buffer)
    {
        // completeCount %d
        int completeCount = Math.Min(CompleteCount.Count, MaxCompleteCount);
        if (completeCount > 0)
        {
            WriteTlvInt32(buffer, 5, completeCount * 1);
            for (int i = 0; i < completeCount; i++)
            {
                WriteInt32(buffer, CompleteCount[i]);
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

        // count
        WriteTlvInt32(buffer, 3, Count);
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}