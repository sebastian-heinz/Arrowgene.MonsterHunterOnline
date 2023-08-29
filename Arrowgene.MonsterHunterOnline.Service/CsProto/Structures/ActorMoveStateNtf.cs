using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 定时位置同步消息
    /// </summary>
    public class ActorMoveStateNtf : Structure, ICsStructure
    {
        public ActorMoveStateNtf()
        {
            NetObjId = 0;
            ActorMoveState = new ActorMoveState();
        }

        /// <summary>
        /// 需要同步的Actor的NetObjId
        /// </summary>
        public uint NetObjId { get; set; }

        /// <summary>
        /// 定时位置同步消息
        /// </summary>
        public ActorMoveState ActorMoveState { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteUInt32(buffer, NetObjId);
            WriteCsStructure(buffer, ActorMoveState);
        }

        public void ReadCs(IBuffer buffer)
        {
            NetObjId = ReadUInt32(buffer);
            ActorMoveState = ReadCsStructure(buffer, ActorMoveState);
        }
    }
}