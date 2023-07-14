using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 付费重新捏脸请求
    /// </summary>
    public class ModifyFaceReq : Structure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(ModifyFaceReq));

        public ModifyFaceReq()
        {
            Uin = 0;
            RoleIndex = 0;
            ChangeGender = 0;
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
        }

        /// <summary>
        /// Apparently this should be the QQ number, but it seems to be "RoleIndex" too.
        /// </summary>
        public uint Uin { get; set; }

        /// <summary>
        /// Originally this is defined as "DbId" however testing shows that it returns the "RoleIndex"
        /// defined via the "ListRoleRsp"
        /// </summary>
        public ulong RoleIndex { get; set; }

        /// <summary>
        /// 角色是否变性
        /// </summary>
        public ushort ChangeGender { get; set; }

        /// <summary>
        /// 角色性别
        /// </summary>
        public ushort Gender { get; set; }

        /// <summary>
        /// 脸型ID
        /// </summary>
        public ushort FaceId { get; set; }

        /// <summary>
        /// 发型ID
        /// </summary>
        public ushort HairId { get; set; }

        /// <summary>
        /// 内衣ID
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
        /// 捏脸数据集合
        /// </summary>
        public short[] FacialInfo { get; }

        public override void Write(IBuffer buffer)
        {
            WriteUInt32(buffer, Uin);
            WriteUInt64(buffer, RoleIndex);
            WriteUInt16(buffer, ChangeGender);
            WriteUInt16(buffer, Gender);
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
        }

        public override void Read(IBuffer buffer)
        {
            Uin = ReadUInt32(buffer);
            RoleIndex = ReadUInt64(buffer);
            ChangeGender = ReadUInt16(buffer);
            Gender = ReadUInt16(buffer);
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
}