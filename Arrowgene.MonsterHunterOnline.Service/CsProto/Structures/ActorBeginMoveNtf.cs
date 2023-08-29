using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 开始移动消息
    /// </summary>
    public class ActorBeginMoveNtf : Structure, ICsStructure
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

        public  void WriteCs(IBuffer buffer)
        {
            WriteUInt32(buffer, NetObjId);
            WriteCsStructure(buffer, ActorBeginMove);
        }

        public void ReadCs(IBuffer buffer)
        {
            NetObjId = ReadUInt32(buffer);
            ActorBeginMove = ReadCsStructure(buffer, ActorBeginMove);
        }
    }
}