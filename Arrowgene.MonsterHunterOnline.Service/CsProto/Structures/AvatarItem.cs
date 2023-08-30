using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 角色装备显示物品
    /// </summary>
    public class AvatarItem : Structure, ICsStructure
    {
        public AvatarItem()
        {
            ItemType = 0;
            PosIndex = 0;
            Rank = 0;
            EnhanceRule = 0;
            EnhanceLevel = 0;
            SoltCount = 0;
            GemId = 0;
            BreakLevel = 0;
            ColorIndex = 0;
            FakeShow = 0;
            WakeLevel = 0;
        }

        /// <summary>
        /// 物品类型
        /// </summary>
        public int ItemType { get; set; }

        /// <summary>
        /// 物品位置行
        /// </summary>
        public ushort PosIndex { get; set; }

        /// <summary>
        /// Rank级别
        /// </summary>
        public uint Rank { get; set; }

        /// <summary>
        /// 强化规则
        /// </summary>
        public byte EnhanceRule { get; set; }

        /// <summary>
        /// 强化数据
        /// </summary>
        public byte EnhanceLevel { get; set; }

        /// <summary>
        /// 插孔数
        /// </summary>
        public byte SoltCount { get; set; }

        /// <summary>
        /// 插孔宝石物品
        /// </summary>
        public uint GemId { get; set; }

        /// <summary>
        /// 突破级别
        /// </summary>
        public byte BreakLevel { get; set; }

        /// <summary>
        /// 染色效果
        /// </summary>
        public byte ColorIndex { get; set; }

        /// <summary>
        /// 幻化效果
        /// </summary>
        public uint FakeShow { get; set; }

        /// <summary>
        /// 觉醒层级
        /// </summary>
        public byte WakeLevel { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, ItemType);
            WriteUInt16(buffer, PosIndex);
            WriteUInt32(buffer, Rank);
            WriteByte(buffer, EnhanceRule);
            WriteByte(buffer, EnhanceLevel);
            WriteByte(buffer, SoltCount);
            WriteUInt32(buffer, GemId);
            WriteByte(buffer, BreakLevel);
            WriteByte(buffer, ColorIndex);
            WriteUInt32(buffer, FakeShow);
            WriteByte(buffer, WakeLevel);
        }

        public void ReadCs(IBuffer buffer)
        {
            ItemType = ReadInt32(buffer);
            PosIndex = ReadUInt16(buffer);
            Rank = ReadUInt32(buffer);
            EnhanceRule = ReadByte(buffer);
            EnhanceLevel = ReadByte(buffer);
            SoltCount = ReadByte(buffer);
            GemId = ReadUInt32(buffer);
            BreakLevel = ReadByte(buffer);
            ColorIndex = ReadByte(buffer);
            FakeShow = ReadUInt32(buffer);
            WakeLevel = ReadByte(buffer);
        }
    }
}