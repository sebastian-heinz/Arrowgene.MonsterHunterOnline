using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 选择角色请求
    /// </summary>
    public class SelectRoleReq : Structure, ICsStructure
    {
        public SelectRoleReq()
        {
            RoleIndex = 0;
            MacAddress = "";
        }

        /// <summary>
        /// 角色Index
        /// </summary>
        public int RoleIndex { get; set; }

        /// <summary>
        /// 客户端mac地址
        /// </summary>
        public string MacAddress { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, RoleIndex);
            WriteString(buffer, MacAddress);
        }

        public void ReadCs(IBuffer buffer)
        {
            RoleIndex = ReadInt32(buffer);
            MacAddress = ReadString(buffer);
        }
    }
}