using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

public class TlvTaskData : Structure, ITlvStructure
{
    private const int TasksMaxSize = 0x80;

    public TlvTaskData()
    {
        Tasks = new List<TlvTask>();
    }

    public List<TlvTask> Tasks { get; }

    public void WriteTlv(IBuffer buffer)
    {

        // taskCount
        // task
        // contentCount
        // content
        // completeBitCount
        // completeBit
        // daily
        // schedule
        // xDailyCount
        // xDaily
        // reset
        // trace
        // complete


        int maxTasks = Math.Min(Tasks.Count, TasksMaxSize);

        // case 1
        WriteTlvInt32(buffer, 1, maxTasks);

        // case 2
        if (maxTasks > 0)
        {
            WriteTlvSubStructureList(buffer, 2, maxTasks, Tasks);
            // TODO conditional write, seems not to happen under normal circumstances
            // WriteInt32(buffer, 0);
        }

    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}