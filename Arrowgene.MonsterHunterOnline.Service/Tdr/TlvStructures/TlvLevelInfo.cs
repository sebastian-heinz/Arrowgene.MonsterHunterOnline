using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+0x217820
/// </summary>
public class TlvLevelInfo : Structure, ITlvStructure
{
    /// <summary>
    /// crygame.dll+0x216470
    /// </summary>
    public class BranchListEntry : Structure, ITlvStructure
    {
        public byte BranchType { get; set; }
        public short BranchLevel { get; set; }
        public int BranchAllScore { get; set; }
        public int BranchDayScore { get; set; }
        public int BranchRecordCardLevelUpTimes { get; set; }
        public int BranchChallengeScore { get; set; }

        public void WriteTlv(IBuffer buffer)
        {
            WriteTlvByte(buffer, 1, BranchType);
            WriteTlvInt16(buffer, 2, BranchLevel);
            WriteTlvInt32(buffer, 3, BranchAllScore);
            WriteTlvInt32(buffer, 4, BranchDayScore);
            WriteTlvInt32(buffer, 5, BranchRecordCardLevelUpTimes);
            WriteTlvInt32(buffer, 6, BranchChallengeScore);
        }

        public void ReadTlv(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// crygame.dll+0x216C90
    /// </summary>
    public class StarListEntry : Structure, ITlvStructure
    {
        public byte Quality { get; set; }
        public int FinishTime { get; set; }

        public void WriteTlv(IBuffer buffer)
        {
            WriteTlvByte(buffer, 1, Quality);
            WriteTlvInt32(buffer, 2, FinishTime);
        }

        public void ReadTlv(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }

    private const byte MaxStarCount = 0x14;
    private const byte MaxBranchCount = 0xA;

    public TlvLevelInfo()
    {
    }

    public List<BranchListEntry> Branches { get; set; }
    public List<StarListEntry> Stars { get; set; }

    public void WriteTlv(IBuffer buffer)
    {
        int startPos = buffer.Position;
        WriteInt32(buffer, 0);

        //starNum
        byte starNum = (byte)Stars.Count;
        if (starNum > MaxStarCount)
        {
            starNum = MaxStarCount;
        }

        WriteTlvByte(buffer, 1, starNum);

        //branchNum
        byte branchNum = (byte)Branches.Count;
        if (branchNum > MaxBranchCount)
        {
            branchNum = MaxBranchCount;
        }

        WriteTlvByte(buffer, 3, branchNum);

        //branchList
        WriteTlvSubStructureList(buffer, 4, MaxBranchCount, Branches);

        //starList
        WriteTlvSubStructureList(buffer, 5, MaxStarCount, Stars);

        //quality
        //finishTime

        //finishTime?


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