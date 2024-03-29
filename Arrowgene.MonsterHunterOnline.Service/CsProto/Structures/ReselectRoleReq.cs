using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 小退请求
    /// </summary>
    public class ReselectRoleReq : Structure, ICsStructure
    {
        public ReselectRoleReq()
        {
            RoleId = 0;
        }

        /// <summary>
        /// 当前角色ID
        /// </summary>
        public int RoleId { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, RoleId);
        }

        public void ReadCs(IBuffer buffer)
        {
            RoleId = ReadInt32(buffer);
        }
    }
}