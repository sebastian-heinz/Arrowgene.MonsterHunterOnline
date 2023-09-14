using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 进入战斗副本响应
    /// </summary>
    public class EnterInstanceRsp : Structure, ICsStructure
    {
        public EnterInstanceRsp()
        {
            ErrNo = 0;
            RoleId = 0;
            InstanceId = 0;
            BattleSvr = "";
            ServiceId = 0;
            Key = "";
            InstanceInfo = new InstanceInitInfo();
            SameBS = 0;
            CrossRegion = 0;
            MatchRoom = 0;
        }

        /// <summary>
        /// 响应码, 0为成功
        /// </summary>
        public int ErrNo { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 副本实例ID
        /// </summary>
        public int InstanceId { get; set; }

        /// <summary>
        /// BattleSvr连接URL
        /// </summary>
        public string BattleSvr { get; set; }

        /// <summary>
        /// Battlesvr的serviceID
        /// </summary>
        public int ServiceId { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Key;

        public InstanceInitInfo InstanceInfo { get; set; }

        /// <summary>
        /// 是否切换BS
        /// </summary>
        public byte SameBS { get; set; }

        /// <summary>
        /// 是否跨区
        /// </summary>
        public byte CrossRegion { get; set; }

        /// <summary>
        /// 是否匹配
        /// </summary>
        public byte MatchRoom { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, ErrNo);
            WriteInt32(buffer, RoleId);
            WriteInt32(buffer, InstanceId);
            WriteString(buffer, BattleSvr);
            WriteInt32(buffer, ServiceId);
            WriteString(buffer, Key);
            WriteCsStructure(buffer, InstanceInfo);
            WriteByte(buffer, SameBS);
            WriteByte(buffer, CrossRegion);
            WriteByte(buffer, MatchRoom);
        }

        public void ReadCs(IBuffer buffer)
        {
            ErrNo = ReadInt32(buffer);
            RoleId = ReadInt32(buffer);
            InstanceId = ReadInt32(buffer);
            BattleSvr = ReadString(buffer);
            ServiceId = ReadInt32(buffer);
            Key = ReadString(buffer);
            InstanceInfo = ReadCsStructure(buffer, InstanceInfo);
            SameBS = ReadByte(buffer);
            CrossRegion = ReadByte(buffer);
            MatchRoom = ReadByte(buffer);
        }
    }
}