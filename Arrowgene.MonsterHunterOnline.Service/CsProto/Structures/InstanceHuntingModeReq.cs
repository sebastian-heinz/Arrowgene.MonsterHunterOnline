using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 副本狩猎模式选择
    /// </summary>
    public class InstanceHuntingModeReq : Structure, ICsStructure
    {
        public InstanceHuntingModeReq()
        {
            LevelId = 0;
            HuntingMode = 0;
        }

        /// <summary>
        /// level id
        /// </summary>
        public int LevelId { get; set; }

        /// <summary>
        /// 狩猎模式 0狩猎券 1契约金
        /// </summary>
        public int HuntingMode { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, LevelId);
            WriteInt32(buffer, HuntingMode);
        }

        public void ReadCs(IBuffer buffer)
        {
            LevelId = ReadInt32(buffer);
            HuntingMode = ReadInt32(buffer);
        }
    }
}