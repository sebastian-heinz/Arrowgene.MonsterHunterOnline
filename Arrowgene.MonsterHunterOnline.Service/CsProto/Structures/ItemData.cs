using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;
using Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 物品数据
    /// </summary>
    public class ItemData : Structure
    {
        public ItemData()
        {
            ItemType = 0;
            ItemColumn = 0;
            ItemGrid = 0;
            Item = new GeneralItem();
        }

        public ItemData(Item item)
        {
            ItemType = (uint)item.ItemId;
            ItemColumn = item.PosColumn;
            ItemGrid = (ushort)item.PosGrid;
            Item = new GeneralItem();
            Item.ItemId = item.Id;
            Item.ItemType = item.ItemId;
            Item.PosColumn = item.PosColumn;
            Item.PosGrid = item.PosGrid;
            Item.Quantity = item.Quantity;

            // TODO not supported
            // Item.Bind = item.Info.Bind;
            // Item.ItemExtAttrIds.AddRange(item.ItemExtAttrIds);
            // Item.ItemExtAttrVals.AddRange(item.ItemExtAttrVals);
        }

        /// <summary>
        /// 物品类型
        /// </summary>
        public uint ItemType { get; set; }

        /// <summary>
        /// 物品栏位
        /// </summary>
        public ItemColumnType ItemColumn { get; set; }

        /// <summary>
        /// 物品格子
        /// </summary>
        public ushort ItemGrid { get; set; }

        /// <summary>
        /// 物品打包数据
        /// </summary>
        public GeneralItem Item { get; }

        public override void Write(IBuffer buffer)
        {
            WriteUInt32(buffer, ItemType);
            WriteByte(buffer, (byte)ItemColumn);
            WriteUInt16(buffer, ItemGrid);
            WriteTlvStructure(buffer, Item, (ushort)CsProtoConstant.CS_ITEM_DATA_MAX, WriteUInt16);
        }

        public override void Read(IBuffer buffer)
        {
            ItemType = ReadUInt32(buffer);
            ItemColumn = (ItemColumnType)ReadByte(buffer);
            ItemGrid = ReadUInt16(buffer);
            ReadTlvStructure(buffer, Item, (ushort)CsProtoConstant.CS_ITEM_DATA_MAX, ReadUInt16);
        }
    }
}