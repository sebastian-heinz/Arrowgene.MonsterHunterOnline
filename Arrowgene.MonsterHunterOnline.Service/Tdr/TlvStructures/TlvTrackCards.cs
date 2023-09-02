using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+0x21C6D0
/// </summary>
public class TlvTrackCards : Structure, ITlvStructure
{
    private const short MaxCardCount = 0xA;

    public TlvTrackCards()
    {
        Cards = new List<int>();
    }

    public List<int> Cards { get; }

    public void WriteTlv(IBuffer buffer)
    {
        int count = Cards.Count;
        if (count > MaxCardCount)
        {
            count = MaxCardCount;
        }

        //count
        WriteTlvInt32(buffer, 1, count);
        // cards
        if (count > 0)
        {
            WriteTlvInt32(buffer, 2, count * 4);
            for (int i = 0; i < count; i++)
            {
                WriteInt32(buffer, Cards[i]);
            }
        }
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}