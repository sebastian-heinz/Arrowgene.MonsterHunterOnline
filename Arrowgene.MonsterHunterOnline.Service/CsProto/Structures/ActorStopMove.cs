using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 定时位置同步消息
    /// </summary>
    public class ActorStopMove : Structure
    {
        public ActorStopMove()
        {
            SyncTime = 0;
            Location = new CSVec3();
            ActorRot = new CSQuat();
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
        /// Actor朝向
        /// </summary>
        public CSQuat ActorRot { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteInt64(buffer,SyncTime);
            WriteStructure(buffer,Location);
            WriteStructure(buffer,ActorRot);
        }

        public override void Read(IBuffer buffer)
        {
            SyncTime = ReadInt64(buffer);
            ReadStructure(buffer,Location);
            ReadStructure(buffer,ActorRot);
        }
    }
}