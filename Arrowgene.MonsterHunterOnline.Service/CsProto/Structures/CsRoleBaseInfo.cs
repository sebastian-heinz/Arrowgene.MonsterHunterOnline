using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 角色基本信息
/// </summary>
public class CsRoleBaseInfo : IStructure
{
    public CsRoleBaseInfo()
    {
        RoleId = 0;
        RoleIndex = 0;
        Name = "";
        Gender = 1;
        Level = 1;
        RoleState = 1;
        RoleStateEndLeftTime = 1;
        AvatarSetId = 1;
        FaceId = 1;
        HairId = 1;
        UnderclothesId = 1;
        SkinColor = 1;
        HairColor = 1;
        InnerColor = 1;
        EyeBall = 1;
        EyeColor = 1;
        FaceTattooIndex = 0;
        FaceTattooColor = 0;
        AvatarItems = new List<CsAvatarItem>();
        HideHelm = 0;
        HideFashion = 0;
        HideSuite = 0;
        FacialInfo = new short[CsConstant.CS_MAX_FACIALINFO_COUNT];
        StarLevel = "";
        HRLevel = 0;
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
    public List<CsAvatarItem> AvatarItems { get; }

    /// <summary>
    /// 是否隐藏头盔
    /// </summary>
    public byte HideHelm { get; set; }

    /// <summary>
    /// 是否隐藏时装
    /// </summary>
    public byte HideFashion { get; set; }

    /// <summary>
    /// 是否隐藏套件
    /// </summary>
    public byte HideSuite { get; set; }

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
    public int HRLevel { get; set; }

    /// <summary>
    /// 兽魂石等级
    /// </summary>
    public int SoulStoneLv { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteUInt64(RoleId, Endianness.Big);
        buffer.WriteInt32(RoleIndex, Endianness.Big);

        buffer.WriteInt32(Name.Length + 1, Endianness.Big);
        buffer.WriteCString(Name);

        buffer.WriteByte(Gender);
        buffer.WriteInt32(Level, Endianness.Big);
        buffer.WriteInt32(RoleState, Endianness.Big);
        buffer.WriteUInt32(RoleStateEndLeftTime, Endianness.Big);
        buffer.WriteByte(AvatarSetId);
        buffer.WriteUInt16(FaceId, Endianness.Big);
        buffer.WriteUInt16(HairId, Endianness.Big);
        buffer.WriteUInt16(UnderclothesId, Endianness.Big);
        
        ushort equipCount = (ushort)AvatarItems.Count;
        buffer.WriteUInt16(equipCount, Endianness.Big);
        
        buffer.WriteInt32(SkinColor, Endianness.Big);
        buffer.WriteInt32(HairColor, Endianness.Big);
        buffer.WriteInt32(InnerColor, Endianness.Big);
        buffer.WriteInt32(EyeBall, Endianness.Big);
        buffer.WriteInt32(EyeColor, Endianness.Big);
        buffer.WriteInt32(FaceTattooIndex, Endianness.Big);
        buffer.WriteInt32(FaceTattooColor, Endianness.Big);

        for (int i = 0; i < equipCount; i++)
        {
            AvatarItems[i].Write(buffer);
        }

        buffer.WriteByte(HideHelm);
        buffer.WriteByte(HideFashion);
        buffer.WriteByte(HideSuite);

        for (int i = 0; i < CsConstant.CS_MAX_FACIALINFO_COUNT; i++)
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