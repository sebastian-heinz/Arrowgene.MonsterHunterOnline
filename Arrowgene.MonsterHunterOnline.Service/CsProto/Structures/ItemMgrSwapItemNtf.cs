using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 通知交换物品
    /// </summary>
    public class ItemMgrSwapItemNtf : Structure, ICsStructure
    {
        public ItemMgrSwapItemNtf()
        {
            ItemId = 0;
            ItemColumn = 0;
            ItemGrid = 0;
            DstItemId = 0;
            DstColumn = 0;
            DstGrid = 0;
        }

        /// <summary>
        /// 物品实例
        /// </summary>
        public ulong ItemId { get; set; }

        /// <summary>
        /// 物品栏位
        /// </summary>
        public ItemColumnType ItemColumn { get; set; }

        /// <summary>
        /// 物品格子
        /// </summary>
        public ushort ItemGrid { get; set; }

        /// <summary>
        /// 目标物品
        /// </summary>
        public ulong DstItemId { get; set; }

        /// <summary>
        /// 目标栏位
        /// </summary>
        public ItemColumnType DstColumn { get; set; }

        /// <summary>
        /// 目标格子
        /// </summary>
        public ushort DstGrid { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteUInt64(buffer, ItemId);
            WriteByte(buffer, (byte)ItemColumn);
            WriteUInt16(buffer, ItemGrid);
            WriteUInt64(buffer, DstItemId);
            WriteByte(buffer, (byte)DstColumn);
            WriteUInt16(buffer, DstGrid);
        }

        public void ReadCs(IBuffer buffer)
        {
            ItemId = ReadUInt64(buffer);
            ItemColumn = (ItemColumnType)ReadByte(buffer);
            ItemGrid = ReadUInt16(buffer);
            DstItemId = ReadUInt64(buffer);
            DstColumn = (ItemColumnType)ReadByte(buffer);
            DstGrid = ReadUInt16(buffer);
        }
    }
}