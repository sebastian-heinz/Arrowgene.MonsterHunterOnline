using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 玩家换区申请
    /// </summary>
    public class PlayerRegionJumpReq : Structure
    {
        public PlayerRegionJumpReq()
        {
            PlayerPos = new CSVec3();
            TriggerName = "";
        }

        /// <summary>
        /// 客户端玩家当前位置
        /// </summary>
        public CSVec3 PlayerPos { get; set; }

        /// <summary>
        /// 切区触发器名称
        /// </summary>
        public string TriggerName { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteStructure(buffer, PlayerPos);
            WriteString(buffer, TriggerName);
        }

        public override void Read(IBuffer buffer)
        {
            PlayerPos = ReadStructure(buffer, PlayerPos);
            TriggerName = ReadString(buffer);
        }
    }
}