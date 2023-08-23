using System;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

public class TlvAttr : TlvStructure
{
    public TlvAttr()
    {
        CharLevel = new int[7];
        CharMaxSta = new int[7];
        CharSpeed = new int[7];
    }

    public int[] CharLevel { get; }
    public int CharSex { get; set; }
    public int CharExp { get; set; }
    public int StarLevel { get; set; }
    public int CharSta { get; set; }
    public int[] CharMaxSta { get; }
    public int[] CharSpeed { get; }

    public void SetCharLevel(int val)
    {
        SetProp(CharLevel, val);
    }

    public void SetCharSpeed(int val)
    {
        SetProp(CharSpeed, val);
    }

    public void SetCharMaxSta(int val)
    {
        SetProp(CharMaxSta, val);
    }

    public override void Write(IBuffer buffer)
    {
        WriteByte(buffer, (byte)TlvMagic.NoVariant);
        int startPos = buffer.Position;
        WriteInt32(buffer, 0);

        WriteTlvInt32Arr(buffer, 2, CharLevel);
        WriteTlvInt32(buffer, 4, CharSex);
        WriteTlvInt32(buffer, 6, CharExp);
        WriteTlvInt32(buffer, 7, StarLevel);
        WriteTlvInt32(buffer, 21, CharSta);
        WriteTlvInt32Arr(buffer, 22, CharMaxSta);
        WriteTlvInt32Arr(buffer, 74, CharSpeed);

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

    private void SetProp(int[] prop, int val)
    {
        for (int i = 0; i < prop.Length; i++)
        {
            prop[i] = val;
        }
    }
}