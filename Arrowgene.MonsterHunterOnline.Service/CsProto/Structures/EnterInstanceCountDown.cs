using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 进入战斗副本倒计时
    /// </summary>
    public class EnterInstanceCountDown : Structure, ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(EnterInstanceCountDown));

        public EnterInstanceCountDown()
        {
            Second = 0;
            LevelId = 0;
        }

        /// <summary>
        /// 时间
        /// </summary>
        public int Second { get; set; }

        /// <summary>
        /// 关卡ID
        /// </summary>
        public int LevelId { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, Second);
            WriteInt32(buffer, LevelId);
        }

        public void ReadCs(IBuffer buffer)
        {
            Second = ReadInt32(buffer);
            LevelId = ReadInt32(buffer);
        }
    }
}