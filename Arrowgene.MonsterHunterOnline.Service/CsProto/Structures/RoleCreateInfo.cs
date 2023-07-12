using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class RoleCreateInfo : Structure
{
    public RoleCreateInfo()
    {
        Name = "";
        Gender = 0;
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
        FacialInfo = new short[CsProtoConstant.CS_MAX_FACIALINFO_COUNT];
        DbId = 0;
    }

    /// <summary>
    /// 角色名
    /// </summary>
    public string Name;

    /// <summary>
    /// 角色性别
    /// </summary>
    public byte Gender;

    /// <summary>
    /// 脸型ID
    /// </summary>
    public ushort FaceId;

    /// <summary>
    /// 发型ID
    /// </summary>
    public ushort HairId;

    /// <summary>
    /// 内衣ID
    /// </summary>
    public ushort UnderclothesId;

    /// <summary>
    /// 皮肤颜色
    /// </summary>
    public int SkinColor;

    /// <summary>
    /// 头发颜色
    /// </summary>
    public int HairColor;

    /// <summary>
    /// 内衣颜色
    /// </summary>
    public int InnerColor;

    /// <summary>
    /// 眼睛
    /// </summary>
    public int EyeBall;

    /// <summary>
    /// 眼睛颜色
    /// </summary>
    public int EyeColor;

    /// <summary>
    /// 脸部贴花
    /// </summary>
    public int FaceTattooIndex;

    /// <summary>
    /// 脸部贴花颜色
    /// </summary>
    public int FaceTattooColor;

    /// <summary>
    /// 捏脸数据集合
    /// </summary>
    public short[] FacialInfo;

    /// <summary>
    /// DBID,客户端不必填写,从NameSvr返回后查找
    /// </summary>
    public ulong DbId;

    public override void Write(IBuffer buffer)
    {
        WriteString(buffer, Name);
        WriteByte(buffer, Gender);
        WriteUInt16(buffer, FaceId);
        WriteUInt16(buffer, HairId);
        WriteUInt16(buffer, UnderclothesId);
        WriteInt32(buffer, SkinColor);
        WriteInt32(buffer, HairColor);
        WriteInt32(buffer, InnerColor);
        WriteInt32(buffer, EyeBall);
        WriteInt32(buffer, EyeColor);
        WriteInt32(buffer, FaceTattooIndex);
        WriteInt32(buffer, FaceTattooColor);
        WriteArray(buffer, FacialInfo, CsProtoConstant.CS_MAX_FACIALINFO_COUNT, WriteInt16);
        WriteUInt64(buffer, DbId);
    }

    public override void Read(IBuffer buffer)
    {
        Name = ReadString(buffer);
        Gender = ReadByte(buffer);
        FaceId = ReadUInt16(buffer);
        HairId = ReadUInt16(buffer);
        UnderclothesId = ReadUInt16(buffer);
        SkinColor = ReadInt32(buffer);
        HairColor = ReadInt32(buffer);
        InnerColor = ReadInt32(buffer);
        EyeBall = ReadInt32(buffer);
        EyeColor = ReadInt32(buffer);
        FaceTattooIndex = ReadInt32(buffer);
        FaceTattooColor = ReadInt32(buffer);
        ReadArray(buffer, FacialInfo, CsProtoConstant.CS_MAX_FACIALINFO_COUNT, ReadInt16);
    }
}