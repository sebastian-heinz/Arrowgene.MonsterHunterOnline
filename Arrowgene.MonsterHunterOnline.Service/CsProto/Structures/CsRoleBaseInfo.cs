using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class CsRoleBaseInfo
{
    private const int CS_MAX_FACIALINFO_COUNT = 46;

    public CsRoleBaseInfo()
    {
        AvatarItems = new List<CsAvatarItem>();
        FacialInfo = new short[CS_MAX_FACIALINFO_COUNT];
        RoleID = 0;
        RoleIndex = 0;
        Name = "";
        Gender = 1;
        Level = 1;
        RoleState = 1;
        RoleStateEndLeftTime = 1;
        AvatarSetID = 1;
        FaceId = 1;
        HairId = 1;
        UnderclothesId = 1;
        SkinColor = 1;
        HairColor = 1;
        InnerColor = 1;
        EyeBall = 1;
        EyeColor = 1;
    }

    public ulong RoleID { get; set; }
    public int RoleIndex { get; set; }
    public string Name { get; set; }
    public byte Gender { get; set; }
    public int Level { get; set; }
    public int RoleState { get; set; }
    public uint RoleStateEndLeftTime { get; set; }
    public byte AvatarSetID { get; set; }
    public ushort FaceId { get; set; }
    public ushort HairId { get; set; }
    public ushort UnderclothesId { get; set; }
    public ushort EquipCount => (ushort)AvatarItems.Count;
    public int SkinColor { get; set; }
    public int HairColor { get; set; }
    public int InnerColor { get; set; }
    public int EyeBall { get; set; }
    public int EyeColor { get; set; }
    public int FaceTattooIndex { get; set; }
    public int FaceTattooColor { get; set; }
    public List<CsAvatarItem> AvatarItems { get; }
    public byte HideHelm { get; set; }
    public byte HideFashion { get; set; }
    public byte HideSuite { get; set; }
    public short[] FacialInfo { get; }
    public string StarLevel { get; set; }
    public int HRLevel { get; set; }
    public int SoulStoneLv { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteUInt64(RoleID, Endianness.Big);
        buffer.WriteInt32(RoleIndex, Endianness.Big);

        buffer.WriteInt32(Name.Length + 1, Endianness.Big);
        buffer.WriteCString(Name);

        buffer.WriteByte(Gender);
        buffer.WriteInt32(Level, Endianness.Big);
        buffer.WriteInt32(RoleState, Endianness.Big);
        buffer.WriteUInt32(RoleStateEndLeftTime, Endianness.Big);
        buffer.WriteByte(AvatarSetID);
        buffer.WriteUInt16(FaceId, Endianness.Big);
        buffer.WriteUInt16(HairId, Endianness.Big);
        buffer.WriteUInt16(UnderclothesId, Endianness.Big);
        buffer.WriteUInt16(EquipCount, Endianness.Big);
        buffer.WriteInt32(SkinColor, Endianness.Big);
        buffer.WriteInt32(HairColor, Endianness.Big);
        buffer.WriteInt32(InnerColor, Endianness.Big);
        buffer.WriteInt32(EyeBall, Endianness.Big);
        buffer.WriteInt32(EyeColor, Endianness.Big);
        buffer.WriteInt32(FaceTattooIndex, Endianness.Big);
        buffer.WriteInt32(FaceTattooColor, Endianness.Big);

        for (int i = 0; i < EquipCount; i++)
        {
            AvatarItems[i].Write(buffer);
        }

        buffer.WriteByte(HideHelm);
        buffer.WriteByte(HideFashion);
        buffer.WriteByte(HideSuite);

        for (int i = 0; i < CS_MAX_FACIALINFO_COUNT; i++)
        {
            buffer.WriteInt16(FacialInfo[i], Endianness.Big);
        }

        //StarLevel
        buffer.WriteInt32(5, Endianness.Big);
        buffer.WriteCString("Star");

        buffer.WriteInt32(1, Endianness.Big);
        buffer.WriteInt32(1, Endianness.Big);
    }
}