using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 离开战斗副本响应
    /// </summary>
    public class LeaveInstanceRsp : Structure, ICsStructure
    {
        public LeaveInstanceRsp()
        {
            ErrNo = 0;
        }

        /// <summary>
        /// 响应码, 0为成功
        /// </summary>
        public int ErrNo { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, ErrNo);
        }

        public void ReadCs(IBuffer buffer)
        {
            ErrNo = ReadInt32(buffer);
        }
    }
}