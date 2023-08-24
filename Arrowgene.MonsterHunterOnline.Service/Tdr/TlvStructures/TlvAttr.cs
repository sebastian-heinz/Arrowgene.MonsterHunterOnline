using System;
using System.Data.Entity.Core.Metadata.Edm;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

public class TlvAttr : TlvStructure
{
    public TlvAttr()
    {
        CharLevel = new int[7];
        CharMaxSta = new int[7];
        CharSpeed = new int[7];
        CharMaxHP = new int[7];
    }

    public int[] CharLevel { get; }
    public int CharSex { get; set; }
    public int CharExp { get; set; }
    public int StarLevel { get; set; }
    public int CharHP { get; set; }
    public int[] CharMaxHP { get; set; }
    public int CharSta { get; set; }
    public int[] CharMaxSta { get; }
    public int[] CharSpeed { get; }
    public int MaleFace { get; set; }
    public int MaleHair { get; set; }
    public int UnderClothes { get; set; }
    public int SkinColor { get; set; }
    public int HairColor { get; set; }
    public int InnerColor { get; set; }
    public int FaceTattooId { get; set; }
    public int EyeBall { get; set; }
    public int FaceTattooColor { get; set; }
    public int EyeColor { get; set; }
    public bool HideFashion { get; set; }
    public bool HideSuite { get; set; }
    public bool HideHelm { get; set; }
    public int SystemUnlockData { get; set; }
    public int SystemUnlockExtData1 { get; set; }
    public short[] FacialInfo { get; set; }
    public int CharHRLevel { get; set; }
    public int CharHRPoint { get; set; }

    public void SetCharLevel(int val)
    {
        SetProp(CharLevel, val);
    }

    public void SetCharSpeed(int val)
    {
        SetProp(CharSpeed, val);
    }

    public void SetCharMaxHP(int val)
    {
        SetProp(CharMaxHP, val);
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
        WriteTlvInt32(buffer, 16, CharHP);
        WriteTlvInt32Arr(buffer, 17, CharMaxHP);
        WriteTlvInt32(buffer, 21, CharSta);
        WriteTlvInt32Arr(buffer, 22, CharMaxSta);
        WriteTlvInt32Arr(buffer, 74, CharSpeed);
        WriteTlvInt32(buffer, 108, MaleFace);
        WriteTlvInt32(buffer, 109, MaleHair);
        WriteTlvInt32(buffer, 173, UnderClothes);
        WriteTlvInt32(buffer, 205, SkinColor);
        WriteTlvInt32(buffer, 206, HairColor);
        WriteTlvInt32(buffer, 207, InnerColor);
        WriteTlvInt32(buffer, 208, FaceTattooId);
        WriteTlvInt32(buffer, 209, EyeBall);
        WriteTlvInt32(buffer, 220, FaceTattooColor);
        WriteTlvInt32(buffer, 221, EyeColor);
        //TODO: send a bool into tlv
        //WriteTlvByte(buffer, 227, Convert.ToByte(HideFashion));
        //WriteTlvByte(buffer, 228, Convert.ToByte(HideSuite));
        //WriteTlvByte(buffer, 229, Convert.ToByte(HideHelm));
        //TODO: play with helmet attr to see if this works problem is that TlvItem.cs have an issue, slot isn't working, item stay at slot 0 and at slot 0 armor isn't showed
        //WriteTlvInt32(buffer, 227, 0);
        //WriteTlvInt32(buffer, 228, 0);
        WriteTlvInt32(buffer, 229, 0); // hide helmet
        WriteTlvInt32(buffer, 241, SystemUnlockData);
        WriteTlvInt32(buffer, 303, SystemUnlockExtData1);
        //TODO: do we make a private function for the facialinfo part ?
        //No need to make it re usable, facialinfo is the only one that have multiple infos
        int faceAttrId = 252;
        for (int i = 0; i < CsProtoConstant.CS_MAX_FACIALINFO_COUNT; i++)
        {

            WriteTlvInt16(buffer, faceAttrId, FacialInfo[i]);
            faceAttrId++;
            //There is a jump between the 2 FacialInfo parts
            if (faceAttrId == 277)
            {
                faceAttrId = 329;
            }
        }
        WriteTlvInt32(buffer, 322, CharHRLevel);
        WriteTlvInt32(buffer, 323, CharHRPoint);

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