using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 开始移动消息
    /// </summary>
    public class ActorBeginMove : Structure
    {
        public ActorBeginMove()
        {
            SyncTime = 0;
            Location = new CSVec3();
            Rotation = new CSQuat();
            MoveSpeed = new CSVec3();
        }

        /// <summary>
        /// 同步时间
        /// </summary>
        public long SyncTime { get; set; }

        /// <summary>
        /// 世界坐标系下的位置
        /// </summary>
        public CSVec3 Location { get; set; }

        /// <summary>
        /// 朝向
        /// </summary>
        public CSQuat Rotation { get; set; }

        /// <summary>
        /// 移动速度
        /// </summary>
        public CSVec3 MoveSpeed { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteInt64(buffer, SyncTime);
            WriteStructure(buffer, Location);
            WriteStructure(buffer, Rotation);
            WriteStructure(buffer, MoveSpeed);
        }

        public override void Read(IBuffer buffer)
        {
            SyncTime = ReadInt64(buffer);
            Location = ReadStructure(buffer, Location);
            Rotation = ReadStructure(buffer, Rotation);
            MoveSpeed = ReadStructure(buffer, MoveSpeed);
        }
    }
}