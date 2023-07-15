using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 玩家传送消息
    /// </summary>
    public class PlayerTeleport : Structure
    {
        public PlayerTeleport()
        {
            SyncTime = 0;
            NetObjId = 0;
            Region = 0;
            TargetPos = new CSQuatT();
            ParentGuid = 0;
            InitState = 0;
        }

        /// <summary>
        /// 同步时间
        /// </summary>
        public long SyncTime { get; set; }

        /// <summary>
        /// Actor的NetObjId
        /// </summary>
        public uint NetObjId { get; set; }

        /// <summary>
        /// Region
        /// </summary>
        public int Region { get; set; }

        /// <summary>
        /// 目标点位置和朝向
        /// </summary>
        public CSQuatT TargetPos { get; set; }

        /// <summary>
        /// parent entityGUID
        /// </summary>
        public long ParentGuid { get; set; }

        /// <summary>
        /// 是否重置状态
        /// </summary>
        public byte InitState { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteInt64(buffer, SyncTime);
            WriteUInt32(buffer, NetObjId);
            WriteInt32(buffer, Region);
            WriteStructure(buffer, TargetPos);
            WriteInt64(buffer, ParentGuid);
            WriteByte(buffer, InitState);
        }

        public override void Read(IBuffer buffer)
        {
            SyncTime = ReadInt64(buffer);
            NetObjId = ReadUInt32(buffer);
            Region = ReadInt32(buffer);
            TargetPos = ReadStructure(buffer, TargetPos);
            ParentGuid = ReadInt64(buffer);
            InitState = ReadByte(buffer);
        }
    }
}