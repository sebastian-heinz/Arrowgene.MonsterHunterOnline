using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{

    /// <summary>
    /// 玩家换区结束客户端告知服务器
    /// </summary>
    public class PlayerRegionJumpEnd : Structure
    {

        public PlayerRegionJumpEnd()
        {
            RegionId = 0;
        }

        /// <summary>
        /// 换区后玩家当前所在区编号
        /// </summary>
        public int RegionId { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteInt32(buffer, RegionId);
        }

        public override void Read(IBuffer buffer)
        {
            RegionId = ReadInt32(buffer);
        }

    }
}
