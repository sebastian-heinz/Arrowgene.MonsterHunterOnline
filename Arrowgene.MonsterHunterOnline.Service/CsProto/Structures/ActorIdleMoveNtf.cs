using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 位置更新消息
    /// </summary>
    public class ActorIdleMoveNtf : Structure
    {
        public ActorIdleMoveNtf()
        {
            NetObjId = 0;
            ActorIdleMove = new ActorIdleMove();
        }

        /// <summary>
        /// 需要同步的Actor的NetObjId
        /// </summary>
        public uint NetObjId { get; set; }

        /// <summary>
        /// 位置更新消息
        /// </summary>
        public ActorIdleMove ActorIdleMove { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteUInt32(buffer, NetObjId);
            WriteStructure(buffer, ActorIdleMove);
        }

        public override void Read(IBuffer buffer)
        {
            NetObjId = ReadUInt32(buffer);
            ActorIdleMove = ReadStructure(buffer, ActorIdleMove);
        }
    }
}