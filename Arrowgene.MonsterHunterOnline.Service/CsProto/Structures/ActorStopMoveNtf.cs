using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 定时位置同步消息
    /// </summary>
    public class ActorStopMoveNtf : Structure, ICsStructure
    {
        public ActorStopMoveNtf()
        {
            NetObjId = 0;
            ActorStopMove = new ActorStopMove();
        }

        /// <summary>
        /// 需要同步的Actor的NetObjId
        /// </summary>
        public uint NetObjId { get; set; }

        /// <summary>
        /// 定时位置同步消息
        /// </summary>
        public ActorStopMove ActorStopMove { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteUInt32(buffer, NetObjId);
            WriteCsStructure(buffer, ActorStopMove);
        }

        public void ReadCs(IBuffer buffer)
        {
            NetObjId = ReadUInt32(buffer);
            ActorStopMove = ReadCsStructure(buffer, ActorStopMove);
        }
    }
}