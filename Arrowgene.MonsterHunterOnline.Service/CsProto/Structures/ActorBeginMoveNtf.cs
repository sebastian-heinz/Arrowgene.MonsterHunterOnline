using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 开始移动消息
    /// </summary>
    public class ActorBeginMoveNtf : Structure
    {
        public ActorBeginMoveNtf()
        {
            NetObjId = 0;
            ActorBeginMove = new ActorBeginMove();
        }

        /// <summary>
        /// 需要同步的Actor的NetObjId
        /// </summary>
        public uint NetObjId { get; set; }

        /// <summary>
        /// 开始移动消息
        /// </summary>
        public ActorBeginMove ActorBeginMove { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteUInt32(buffer, NetObjId);
            WriteStructure(buffer, ActorBeginMove);
        }

        public override void Read(IBuffer buffer)
        {
            NetObjId = ReadUInt32(buffer);
            ActorBeginMove = ReadStructure(buffer, ActorBeginMove);
        }
    }
}