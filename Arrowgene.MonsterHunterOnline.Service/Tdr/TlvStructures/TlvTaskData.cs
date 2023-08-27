using System;
using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

public class TlvTaskData : TlvStructure
{
    private const int TasksMaxSize = 0x80;

    public TlvTaskData()
    {
        Tasks = new List<TlvTask>();
    }

    public List<TlvTask> Tasks { get; }

    public override void Write(IBuffer buffer)
    {
        WriteByte(buffer, (byte)TlvMagic.NoVariant);
        int startPos = buffer.Position;
        WriteInt32(buffer, 0);


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
            WriteTlvTag(buffer, 2, TlvType.ID_4_BYTE);
            int subStartPos = buffer.Position;
            WriteInt32(buffer, 0);
            for (int i = 0; i < maxTasks; i++)
            {
                WriteTlvStructure(buffer, Tasks[i]);
            }

            int subEndPos = buffer.Position;
            int subSize = buffer.Position - subStartPos - 4;
            buffer.Position = subStartPos;
            buffer.WriteInt32(subSize, Endianness.Big);
            buffer.Position = subEndPos;
            
            // TODO conditional write, seems not to happen under normal circumstances
            // WriteInt32(buffer, 0);
        }


        int endPos = buffer.Position;
        int size = endPos - startPos + 1;
        buffer.Position = startPos;
        WriteInt32(buffer, size);
        buffer.Position = endPos;
    }

    public override void Read(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}