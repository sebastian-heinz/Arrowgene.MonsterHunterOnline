using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+0x21D840
/// </summary>
public class TlvHunterStar : Structure, ITlvStructure
{
    private const short MaxStatsCount = 0xBB8;

    public class HunterStarStats
    {
        public ushort Id { get; set; }
        public uint Value { get; set; }
    }

    public TlvHunterStar()
    {
        OpenFlag = 0;
        ResetTime = 0;
        RecordCardLevelUpTimes = 0;
        StarScore = 0;
        LevelInfo = new TlvLevelInfo();
        CardInfo = new TlvCardInfo();
        StatInfo = new TlvStatInfo();
        TrackCards = new TlvTrackCards();
        WeeklyRefreshTime = 0;
        Stats = new List<HunterStarStats>();
    }

    public int OpenFlag { get; set; }
    public int ResetTime { get; set; }
    public int RecordCardLevelUpTimes { get; set; }
    public int StarScore { get; set; }
    public TlvLevelInfo LevelInfo { get; set; }
    public TlvCardInfo CardInfo { get; set; }
    public TlvStatInfo StatInfo { get; set; }
    public TlvTrackCards TrackCards { get; set; }
    public int WeeklyRefreshTime { get; set; }
    public List<HunterStarStats> Stats { get; }

    public void WriteTlv(IBuffer buffer)
    {
        int startPos = buffer.Position;
        WriteInt32(buffer, 0);

        WriteTlvInt32(buffer, 2, OpenFlag);
        WriteTlvInt32(buffer, 3, ResetTime);
        WriteTlvInt32(buffer, 4, RecordCardLevelUpTimes);
        WriteTlvInt32(buffer, 5, StarScore);
        WriteTlvSubStructure(buffer, 6, LevelInfo);
        WriteTlvSubStructure(buffer, 7, CardInfo);
        WriteTlvSubStructure(buffer, 8, StatInfo);
        WriteTlvSubStructure(buffer, 9, TrackCards);
        
        WriteTlvInt32(buffer, 10, WeeklyRefreshTime);

        short statsCount = (short)Stats.Count;
        if (statsCount > MaxStatsCount)
        {
            statsCount = MaxStatsCount;
        }

        if (statsCount > 0)
        {
            //newStatCount
            WriteTlvInt16(buffer, 11, statsCount);

            //newStatIds
            WriteTlvInt32(buffer, 12, statsCount * 2);
            for (int i = 0; i < statsCount; i++)
            {
                WriteUInt16(buffer, Stats[i].Id);
            }

            //newStatVals
            WriteTlvInt32(buffer, 13, statsCount * 4);
            for (int i = 0; i < statsCount; i++)
            {
                WriteUInt32(buffer, Stats[i].Value);
            }
        }

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