using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 定时位置同步消息
    /// </summary>
    public class ActorStopMoveNtf : Structure
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

        public override void Write(IBuffer buffer)
        {
            WriteUInt32(buffer, NetObjId);
            WriteStructure(buffer, ActorStopMove);
        }

        public override void Read(IBuffer buffer)
        {
            NetObjId = ReadUInt32(buffer);
            ActorStopMove = ReadStructure(buffer, ActorStopMove);
        }
    }
}