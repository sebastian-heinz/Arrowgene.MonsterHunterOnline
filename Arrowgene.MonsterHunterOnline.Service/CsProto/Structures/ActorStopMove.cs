using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 定时位置同步消息
    /// </summary>
    public class ActorStopMove : Structure, ICsStructure
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

        public  void WriteCs(IBuffer buffer)
        {
            WriteInt64(buffer,SyncTime);
            WriteCsStructure(buffer,Location);
            WriteCsStructure(buffer,ActorRot);
        }

        public void ReadCs(IBuffer buffer)
        {
            SyncTime = ReadInt64(buffer);
            ReadCsStructure(buffer,Location);
            ReadCsStructure(buffer,ActorRot);
        }
    }
}