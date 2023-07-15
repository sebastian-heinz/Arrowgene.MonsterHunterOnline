using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 切换TOWN地图请求
    /// </summary>
    public class ChangeTownInstanceReq : Structure
    {
        public ChangeTownInstanceReq()
        {
            TriggerName = "";
            LevelId = 0;
            DstPoint = "";
        }

        /// <summary>
        /// 切换点的名字
        /// </summary>
        public string TriggerName { get; set; }

        /// <summary>
        /// 目的关卡
        /// </summary>
        public int LevelId { get; set; }

        /// <summary>
        /// 目的点
        /// </summary>
        public string DstPoint { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteString(buffer, TriggerName);
            WriteInt32(buffer, LevelId);
            WriteString(buffer, DstPoint);
        }

        public override void Read(IBuffer buffer)
        {
            TriggerName = ReadString(buffer);
            LevelId = ReadInt32(buffer);
            DstPoint = ReadString(buffer);
        }
    }
}