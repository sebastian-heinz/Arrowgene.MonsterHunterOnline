using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 切换TOWN地图回复
    /// </summary>
    public class ChangeTownInstanceRsp : Structure
    {
        public ChangeTownInstanceRsp()
        {
            ErrCode = 0;
            LevelId = 0;
        }

        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrCode { get; set; }

        /// <summary>
        /// 目标地图
        /// </summary>
        public int LevelId { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteInt32(buffer, ErrCode);
            WriteInt32(buffer, LevelId);
        }

        public override void Read(IBuffer buffer)
        {
            ErrCode = ReadInt32(buffer);
            LevelId = ReadInt32(buffer);
        }
    }
}