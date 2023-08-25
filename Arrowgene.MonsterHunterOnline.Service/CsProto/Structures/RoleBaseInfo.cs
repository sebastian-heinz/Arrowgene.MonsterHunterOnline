using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class RoleBaseInfo : Structure
{
    public RoleBaseInfo()
    {
        RoleId = 0;
        RoleIndex = 0;
        Name = "";
        Gender = 0;
        Level = 0;
        RoleState = 0;
        RoleStateEndLeftTime = 0;
        AvatarSetId = 0;
        FaceId = 0;
        HairId = 0;
        UnderclothesId = 0;
        SkinColor = 0;
        HairColor = 0;
        InnerColor = 0;
        EyeBall = 0;
        EyeColor = 0;
        FaceTattooIndex = 0;
        FaceTattooColor = 0;
        Equip = new List<CSAvatarItem>();
        HideHelm = false;
        HideFashion = false;
        HideSuite = false;
        FacialInfo = new short[CsProtoConstant.CS_MAX_FACIALINFO_COUNT];
        StarLevel = "";
        HrLevel = 0;
        SoulStoneLv = 0;
    }

    /// <summary>
    /// 角色ID
    /// </summary>
    public ulong RoleId { get; set; }

    /// <summary>
    /// 角色Index
    /// </summary>
    public int RoleIndex { get; set; }

    /// <summary>
    /// 角色名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 角色性别
    /// </summary>
    public byte Gender { get; set; }

    /// <summary>
    /// 角色等级
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 角色状态
    /// </summary>
    public int RoleState { get; set; }

    /// <summary>
    /// 角色状态结束的剩余时间
    /// </summary>
    public uint RoleStateEndLeftTime { get; set; }

    /// <summary>
    /// Avatar Set
    /// </summary>
    public byte AvatarSetId { get; set; }

    /// <summary>
    /// 脸型
    /// </summary>
    public ushort FaceId { get; set; }

    /// <summary>
    /// 发型
    /// </summary>
    public ushort HairId { get; set; }

    /// <summary>
    /// 内衣
    /// </summary>
    public ushort UnderclothesId { get; set; }

    /// <summary>
    /// 皮肤颜色
    /// </summary>
    public int SkinColor { get; set; }

    /// <summary>
    /// 头发颜色
    /// </summary>
    public int HairColor { get; set; }

    /// <summary>
    /// 内衣颜色
    /// </summary>
    public int InnerColor { get; set; }

    /// <summary>
    /// 眼睛
    /// </summary>
    public int EyeBall { get; set; }

    /// <summary>
    /// 眼睛颜色
    /// </summary>
    public int EyeColor { get; set; }

    /// <summary>
    /// 脸部贴花
    /// </summary>
    public int FaceTattooIndex { get; set; }

    /// <summary>
    /// 脸部贴花颜色
    /// </summary>
    public int FaceTattooColor { get; set; }

    /// <summary>
    /// 装备物品
    /// </summary>
    public List<CSAvatarItem> Equip { get; }

    /// <summary>
    /// 是否隐藏头盔
    /// </summary>
    public bool HideHelm { get; set; }

    /// <summary>
    /// 是否隐藏时装
    /// </summary>
    public bool HideFashion { get; set; }

    /// <summary>
    /// 是否隐藏套件
    /// </summary>
    public bool HideSuite { get; set; }

    /// <summary>
    /// 捏脸数据集合
    /// </summary>
    public short[] FacialInfo { get; }

    /// <summary>
    /// 星级
    /// </summary>
    public string StarLevel { get; set; }

    /// <summary>
    /// 角色HR等级
    /// </summary>
    public int HrLevel { get; set; }

    /// <summary>
    /// 兽魂石等级
    /// </summary>
    public int SoulStoneLv { get; set; }

    public override void Write(IBuffer buffer)
    {
        WriteUInt64(buffer, RoleId);
        WriteInt32(buffer, RoleIndex);
        WriteString(buffer, Name);
        WriteByte(buffer, Gender);
        WriteInt32(buffer, Level);
        WriteInt32(buffer, RoleState);
        WriteUInt32(buffer, RoleStateEndLeftTime);
        WriteByte(buffer, AvatarSetId);
        WriteUInt16(buffer, FaceId);
        WriteUInt16(buffer, HairId);
        WriteUInt16(buffer, UnderclothesId);
        ushort equipSize = (ushort)Equip.Count;
        if (equipSize > CsProtoConstant.ROLE_EQUIPED_MAX_NETMESSAGE)
        {
            equipSize = CsProtoConstant.ROLE_EQUIPED_MAX_NETMESSAGE;
        }

        WriteUInt16(buffer, equipSize);
        WriteInt32(buffer, SkinColor);
        WriteInt32(buffer, HairColor);
        WriteInt32(buffer, InnerColor);
        WriteInt32(buffer, EyeBall);
        WriteInt32(buffer, EyeColor);
        WriteInt32(buffer, FaceTattooIndex);
        WriteInt32(buffer, FaceTattooColor);
        WriteList(buffer, Equip, equipSize, CsProtoConstant.ROLE_EQUIPED_MAX_NETMESSAGE, WriteStructure);
        WriteBool(buffer, HideHelm);
        WriteBool(buffer, HideFashion);
        WriteBool(buffer, HideSuite);
        WriteArray(buffer, FacialInfo, CsProtoConstant.CS_MAX_FACIALINFO_COUNT, WriteInt16);
        WriteString(buffer, StarLevel);
        WriteInt32(buffer, HrLevel);
        WriteInt32(buffer, SoulStoneLv);
    }

    public override void Read(IBuffer buffer)
    {
        RoleId = ReadUInt64(buffer);
        RoleIndex = ReadInt32(buffer);
        Name = ReadString(buffer);
        Gender = ReadByte(buffer);
        Level = ReadInt32(buffer);
        RoleState = ReadInt32(buffer);
        RoleStateEndLeftTime = ReadUInt32(buffer);
        AvatarSetId = ReadByte(buffer);
        FaceId = ReadUInt16(buffer);
        HairId = ReadUInt16(buffer);
        UnderclothesId = ReadUInt16(buffer);
        ushort equipSize = ReadUInt16(buffer);
        if (equipSize > CsProtoConstant.ROLE_EQUIPED_MAX_NETMESSAGE)
        {
            equipSize = CsProtoConstant.ROLE_EQUIPED_MAX_NETMESSAGE;
        }

        SkinColor = ReadInt32(buffer);
        HairColor = ReadInt32(buffer);
        InnerColor = ReadInt32(buffer);
        EyeBall = ReadInt32(buffer);
        EyeColor = ReadInt32(buffer);
        FaceTattooIndex = ReadInt32(buffer);
        FaceTattooColor = ReadInt32(buffer);
        Equip.Clear();
        ReadList(buffer, Equip, equipSize, CsProtoConstant.ROLE_EQUIPED_MAX_NETMESSAGE, ReadStructure<CSAvatarItem>);
        HideHelm = ReadBool(buffer);
        HideFashion = ReadBool(buffer);
        HideSuite = ReadBool(buffer);
        ReadArray(buffer, FacialInfo, CsProtoConstant.CS_MAX_FACIALINFO_COUNT, ReadInt16);
        StarLevel = ReadString(buffer);
        HrLevel = ReadInt32(buffer);
        SoulStoneLv = ReadInt32(buffer);
    }
}