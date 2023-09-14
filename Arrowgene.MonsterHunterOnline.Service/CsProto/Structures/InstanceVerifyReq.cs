using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 进入战斗副本验证请求
    /// </summary>
    public class InstanceVerifyReq : Structure, ICsStructure
    {
        public InstanceVerifyReq()
        {
            Uin = 0;
            PVPMode = 0;
            VerifyType = 0;
            RoleId = 0;
            ServiceId = 0;
            Key = "";
            ProtoVer = 0;
            Reserve1 = 0;
            Reserve2 = 0;
        }

        /// <summary>
        /// qq号
        /// </summary>
        public uint Uin { get; set; }

        public int PVPMode { get; set; }

        /// <summary>
        /// 验证类型，正常验证0，battle直接连接测试验证1
        /// </summary>
        public int VerifyType { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// TDR连接的BS ID
        /// </summary>
        public int ServiceId { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 客户端当前的协议版本号
        /// </summary>
        public int ProtoVer { get; set; }

        /// <summary>
        /// 保留字段1,由于此条协议用于客户端服务器的版本交换，故不支持版本裁剪
        /// </summary>
        public int Reserve1 { get; set; }

        /// <summary>
        /// 保留字段2,由于此条协议用于客户端服务器的版本交换，故不支持版本裁剪
        /// </summary>
        public int Reserve2 { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteUInt32(buffer, Uin);
            WriteInt32(buffer, PVPMode);
            WriteInt32(buffer, VerifyType);
            WriteInt32(buffer, RoleId);
            WriteInt32(buffer, ServiceId);
            WriteString(buffer, Key);
            WriteInt32(buffer, ProtoVer);
            WriteInt32(buffer, Reserve1);
            WriteInt32(buffer, Reserve2);
        }

        public void ReadCs(IBuffer buffer)
        {
            Uin = ReadUInt32(buffer);
            PVPMode = ReadInt32(buffer);
            VerifyType = ReadInt32(buffer);
            RoleId = ReadInt32(buffer);
            ServiceId = ReadInt32(buffer);
            Key = ReadString(buffer);
            ProtoVer = ReadInt32(buffer);
            Reserve1 = ReadInt32(buffer);
            Reserve2 = ReadInt32(buffer);
        }
    }
}