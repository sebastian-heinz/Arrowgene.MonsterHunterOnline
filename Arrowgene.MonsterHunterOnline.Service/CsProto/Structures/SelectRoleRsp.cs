using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 选择角色响应
    /// </summary>
    public class SelectRoleRsp : Structure
    {
        public SelectRoleRsp()
        {
            ErrNo = 0;
            RoleIndex = 0;
        }

        /// <summary>
        /// 0为成功，-3表示没有可用的TownServer服务器, 其他是错误码
        /// </summary>
        public int ErrNo { get; set; }

        /// <summary>
        /// 角色Index
        /// </summary>
        public int RoleIndex { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteInt32(buffer, ErrNo);
            WriteInt32(buffer, RoleIndex);
        }

        public override void Read(IBuffer buffer)
        {
            ErrNo = ReadInt32(buffer);
            RoleIndex = ReadInt32(buffer);
        }
    }
}