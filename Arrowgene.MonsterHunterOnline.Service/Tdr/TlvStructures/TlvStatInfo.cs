using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+0x21BC10
/// </summary>
public class TlvStatInfo : Structure, ITlvStructure
{
    private const short MaxStatNumInt = 0xC8;
    private const short MaxStatNum = 0xE74;

    /// <summary>
    /// crygame.dll+0x21B0A0
    /// </summary>
    public class StatListIntEntry : Structure, ITlvStructure
    {
        public ushort Index { get; set; }
        public uint Value { get; set; }

        public void WriteTlv(IBuffer buffer)
        {
            WriteTlvUInt16(buffer, 1, Index);
            WriteTlvUInt32(buffer, 2, Value);
        }

        public void ReadTlv(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }


    public class StatListEntry : Structure, ITlvStructure
    {
        public ushort Index { get; set; }
        public ushort Value { get; set; }

        public void WriteTlv(IBuffer buffer)
        {
            WriteTlvUInt16(buffer, 1, Index);
            WriteTlvUInt16(buffer, 2, Value);
        }

        public void ReadTlv(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }

    public TlvStatInfo()
    {
        StatListInt = new List<StatListIntEntry>();
        StatList = new List<StatListEntry>();
    }


    public List<StatListIntEntry> StatListInt { get; set; }
    public List<StatListEntry> StatList { get; set; }

    public void WriteTlv(IBuffer buffer)
    {
        int startPos = buffer.Position;
        WriteInt32(buffer, 0);

        //statNumInt
        short statNumInt = (short)StatListInt.Count;
        if (statNumInt > MaxStatNumInt)
        {
            statNumInt = MaxStatNumInt;
        }

        WriteTlvInt16(buffer, 1, statNumInt);

        //statListInt
        WriteTlvSubStructureList(buffer, 2, MaxStatNumInt, StatListInt);

        //statNum
        short statNum = (short)StatList.Count;
        if (statNum > MaxStatNum)
        {
            statNum = MaxStatNum;
        }

        WriteTlvInt16(buffer, 3, statNum);

        //statList
        WriteTlvSubStructureList(buffer, 4, MaxStatNum, StatList);

        int endPos = buffer.Position;
        int size = endPos - startPos + 1;
        buffer.Position = startPos;
        WriteInt32(buffer, size);
        buffer.Position = endPos;
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}