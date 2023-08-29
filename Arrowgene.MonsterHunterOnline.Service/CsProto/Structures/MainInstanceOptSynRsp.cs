using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 副本主UI操作同步响应
    /// </summary>
    public class MainInstanceOptSynRsp : Structure, ICsStructure
    {
        public MainInstanceOptSynRsp()
        {
            TriggerId = 0;
            InstancePoint = 0;
            LevelId = 0;
            LevelDiff = 0;
            LevelMode = 0;
        }

        /// <summary>
        /// 触发点
        /// </summary>
        public int TriggerId { get; set; }

        /// <summary>
        /// 副本UI点
        /// </summary>
        public int InstancePoint { get; set; }

        /// <summary>
        /// level id
        /// </summary>
        public int LevelId { get; set; }

        /// <summary>
        /// 难度
        /// </summary>
        public int LevelDiff { get; set; }

        /// <summary>
        /// 模式
        /// </summary>
        public int LevelMode { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, TriggerId);
            WriteInt32(buffer, InstancePoint);
            WriteInt32(buffer, LevelId);
            WriteInt32(buffer, LevelDiff);
            WriteInt32(buffer, LevelMode);
        }

        public void ReadCs(IBuffer buffer)
        {
            TriggerId = ReadInt32(buffer);
            InstancePoint = ReadInt32(buffer);
            LevelId = ReadInt32(buffer);
            LevelDiff = ReadInt32(buffer);
            LevelMode = ReadInt32(buffer);
        }
    }
}