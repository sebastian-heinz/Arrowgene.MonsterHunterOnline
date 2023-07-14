using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// TownSession建立
    /// </summary>
    public class TownSessionStart : Structure
    {
        public TownSessionStart()
        {
            ErrNo = 0;
        }

        /// <summary>
        /// 0为成功，其他是错误码
        /// </summary>
        public byte ErrNo { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteByte(buffer, ErrNo);
        }

        public override void Read(IBuffer buffer)
        {
            ErrNo = ReadByte(buffer);
        }
    }
}