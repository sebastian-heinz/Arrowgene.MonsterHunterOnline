using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 玩家换区状态通知
    /// </summary>
    public class PlayerRegionJumpRsp : Structure
    {
        public PlayerRegionJumpRsp()
        {
            ErrorCode = 0;
            RegionId = 0;
            Transform = new CSQuatT();
        }

        /// <summary>
        /// 换区反馈错误编号
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 换区后玩家当前所在区编号
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// 换区后玩家当前所在区位置和朝向
        /// </summary>
        public CSQuatT Transform { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteInt32(buffer, ErrorCode);
            WriteInt32(buffer, RegionId);
            WriteStructure(buffer, Transform);
        }

        public override void Read(IBuffer buffer)
        {
            ErrorCode = ReadInt32(buffer);
            RegionId = ReadInt32(buffer);
            Transform = ReadStructure(buffer, Transform);
        }
    }
}