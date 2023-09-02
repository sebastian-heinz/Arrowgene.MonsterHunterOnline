using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

/// <summary>
/// crygame.dll+0x21A010
/// </summary>
public class TlvCardInfo : Structure, ITlvStructure
{
    private const short MaxUnlockBitCount = 0x9C4;
    private const short MaxCompleteBitCount = 0x9C4;
    private const short MaxFinishCardCount = 0xA;

    public TlvCardInfo()
    {
        UnlockBits = new List<byte>();
        CompleteBits = new List<byte>();
        NewFinishCards = new List<int>();
    }

    public List<byte> UnlockBits { get; }
    public List<byte> CompleteBits { get; }
    public List<int> NewFinishCards { get; }


    public void WriteTlv(IBuffer buffer)
    {
        int unlockBitCount = UnlockBits.Count;
        if (unlockBitCount > MaxUnlockBitCount)
        {
            unlockBitCount = MaxUnlockBitCount;
        }

        //unlockBitCount
        WriteTlvInt32(buffer, 1, unlockBitCount);

        //unlockBit
        if (unlockBitCount > 0)
        {
            WriteTlvInt32(buffer, 2, unlockBitCount * 1);
            for (int i = 0; i < unlockBitCount; i++)
            {
                WriteByte(buffer, UnlockBits[i]);
            }
        }

        int completeBitCount = CompleteBits.Count;
        if (completeBitCount > MaxCompleteBitCount)
        {
            completeBitCount = MaxCompleteBitCount;
        }

        //completeBitCount
        WriteTlvInt32(buffer, 3, completeBitCount);

        //completeBit
        if (completeBitCount > 0)
        {
            WriteTlvInt32(buffer, 4, completeBitCount * 1);
            for (int i = 0; i < completeBitCount; i++)
            {
                WriteByte(buffer, CompleteBits[i]);
            }
        }

        int newFinishCardNum = NewFinishCards.Count;
        if (newFinishCardNum > MaxFinishCardCount)
        {
            newFinishCardNum = MaxFinishCardCount;
        }

        //newFinishCardNum
        WriteTlvInt32(buffer, 5, newFinishCardNum);

        //newFinishCardList
        if (newFinishCardNum > 0)
        {
            WriteTlvInt32(buffer, 6, newFinishCardNum * 4);
            for (int i = 0; i < newFinishCardNum; i++)
            {
                WriteInt32(buffer, NewFinishCards[i]);
            }
        }
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}