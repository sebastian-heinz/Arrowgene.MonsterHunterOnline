using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// TownSession建立
    /// </summary>
    public class TownSessionStart : Structure, ICsStructure
    {
        public TownSessionStart()
        {
            ErrNo = 0;
        }

        /// <summary>
        /// 0为成功，其他是错误码
        /// </summary>
        public byte ErrNo { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteByte(buffer, ErrNo);
        }

        public void ReadCs(IBuffer buffer)
        {
            ErrNo = ReadByte(buffer);
        }
    }
}