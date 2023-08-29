using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 物品数据
    /// </summary>
    public class ItemData : Structure, ICsStructure
    {
        public ItemData(Item item)
        {
            ItemType = item.ItemId;
            ItemColumn = item.PosColumn;
            ItemGrid = item.PosGrid;
            Item = item;
        }

        public ItemData()
        {
            ItemType = 0;
            ItemColumn = 0;
            ItemGrid = 0;
            Item = new Item();
        }

        /// <summary>
        /// 物品类型
        /// </summary>
        public uint ItemType { get; private set; }

        /// <summary>
        /// 物品栏位
        /// </summary>
        public ItemColumnType ItemColumn { get; private set; }

        /// <summary>
        /// 物品格子
        /// </summary>
        public ushort ItemGrid { get; private set; }

        /// <summary>
        /// 物品打包数据
        /// </summary>
        public Item Item { get; }

        public void WriteCs(IBuffer buffer)
        {
            WriteUInt32(buffer, ItemType);
            WriteByte(buffer, (byte)ItemColumn);
            WriteUInt16(buffer, ItemGrid);
            WriteTlvStructure(buffer, Item, (ushort)CsProtoConstant.CS_ITEM_DATA_MAX, WriteUInt16);
        }

        public void ReadCs(IBuffer buffer)
        {
            ItemType = ReadUInt32(buffer);
            ItemColumn = (ItemColumnType)ReadByte(buffer);
            ItemGrid = ReadUInt16(buffer);
            ReadTlvStructure(buffer, Item, (ushort)CsProtoConstant.CS_ITEM_DATA_MAX, ReadUInt16);
        }
    }
}