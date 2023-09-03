using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 副本进入同意请求
    /// </summary>
    public class MainInstanceAgreeOptReq : Structure, ICsStructure
    {
        public MainInstanceAgreeOptReq()
        {
            Agree = 0;
        }

        /// <summary>
        /// 是否同意
        /// </summary>
        public int Agree { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, Agree);
        }

        public void ReadCs(IBuffer buffer)
        {
            Agree = ReadInt32(buffer);
        }
    }
}