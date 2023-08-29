using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 通知移动物品
    /// </summary>
    public class ItemMgrMoveItemNtf : Structure, ICsStructure
    {
        public ItemMgrMoveItemNtf()
        {
            ItemId = 0;
            ItemColumn = 0;
            ItemGrid = 0;
            DstColumn = 0;
            DstGrid = 0;
        }

        /// <summary>
        /// 物品实例
        /// </summary>
        public ulong ItemId;

        /// <summary>
        /// 物品栏位
        /// </summary>
        public ItemColumnType ItemColumn;

        /// <summary>
        /// 物品格子
        /// </summary>
        public ushort ItemGrid;

        /// <summary>
        /// 目标栏位
        /// </summary>
        public ItemColumnType DstColumn;

        /// <summary>
        /// 目标格子
        /// </summary>
        public ushort DstGrid;

        public  void WriteCs(IBuffer buffer)
        {
            WriteUInt64(buffer, ItemId);
            WriteByte(buffer, (byte)ItemColumn);
            WriteUInt16(buffer, ItemGrid);
            WriteByte(buffer, (byte)DstColumn);
            WriteUInt16(buffer, DstGrid);
        }

        public void ReadCs(IBuffer buffer)
        {
            ItemId = ReadUInt64(buffer);
            ItemColumn = (ItemColumnType)ReadByte(buffer);
            ItemGrid = ReadUInt16(buffer);
            DstColumn = (ItemColumnType)ReadByte(buffer);
            DstGrid = ReadUInt16(buffer);
        }
    }
}