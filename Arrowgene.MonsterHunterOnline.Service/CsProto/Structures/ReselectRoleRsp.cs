using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 小退响应
    /// </summary>
    public class ReselectRoleRsp : Structure, ICsStructure
    {
        public ReselectRoleRsp()
        {
            ErrNo = 0;
        }

        /// <summary>
        /// 0为成功，其他是错误码
        /// </summary>
        public uint ErrNo { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteUInt32(buffer, ErrNo);
        }

        public void ReadCs(IBuffer buffer)
        {
            ErrNo = ReadUInt32(buffer);
        }
    }
}