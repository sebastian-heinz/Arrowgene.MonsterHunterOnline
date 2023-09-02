using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    public class ItemListRsp : Structure, ICsStructure, CSICsRemoteDataInfo, IItemListProperties
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(ItemListRsp));

        public ItemListRsp()
        {
            StoreSize = 0;
            NormalSize = 0;
            MaterialStoreSize = 0;
            BagItem = new TlvItemList();
            EquipItem = new TlvItemList();
            StoreItem = new TlvItemList();
            ItemUseOnceList = new List<ushort>();
        }

        public ROMTE_DATA_TYPE DataType => ROMTE_DATA_TYPE.ITEMMGR_DATA_TYPE;

        /// <summary>
        /// 仓库大小
        /// </summary>
        public ushort StoreSize { get; set; }

        /// <summary>
        /// 普通背包容量大小
        /// </summary>
        public ushort NormalSize { get; set; }

        /// <summary>
        /// 素材仓库大小
        /// </summary>
        public ushort MaterialStoreSize { get; set; }

        /// <summary>
        /// 背包道具数据
        /// </summary>
        public TlvItemList BagItem { get; }

        /// <summary>
        /// 装备数据
        /// </summary>
        public TlvItemList EquipItem { get; }

        /// <summary>
        /// 仓库数据
        /// </summary>
        public TlvItemList StoreItem { get; }

        /// <summary>
        /// 唯一使用物品列表
        /// </summary>
        public List<ushort> ItemUseOnceList { get; }

        public void WriteCs(IBuffer buffer)
        {
            WriteUInt16(buffer, StoreSize);
            WriteUInt16(buffer, NormalSize);
            WriteUInt16(buffer, MaterialStoreSize);
            WriteTlvStructure(buffer, BagItem, CsProtoConstant.CS_ITEM_BAG_DATA_LEN, WriteInt32);
            WriteTlvStructure(buffer, EquipItem, (ushort)CsProtoConstant.CS_ITEM_EQUIP_DATA_LEN, WriteUInt16);
            WriteTlvStructure(buffer, StoreItem, CsProtoConstant.CS_ITEM_STORE_DATA_LEN, WriteInt32);

            ushort itemUseOnceListCount = (ushort)ItemUseOnceList.Count;
            buffer.WriteUInt16(itemUseOnceListCount);
            for (int i = 0; i < itemUseOnceListCount; i++)
            {
                buffer.WriteUInt16(ItemUseOnceList[i]);
            }
        }

        public void ReadCs(IBuffer buffer)
        {
            StoreSize = ReadUInt16(buffer);
            NormalSize = ReadUInt16(buffer);
            MaterialStoreSize = ReadUInt16(buffer);
            ReadTlvStructure(buffer, BagItem, CsProtoConstant.CS_ITEM_BAG_DATA_LEN, ReadInt32);
            ReadTlvStructure(buffer, EquipItem, (ushort)CsProtoConstant.CS_ITEM_EQUIP_DATA_LEN, ReadUInt16);
            ReadTlvStructure(buffer, StoreItem, CsProtoConstant.CS_ITEM_STORE_DATA_LEN, ReadInt32);

            ItemUseOnceList.Clear();
            ushort itemUseOnceListCount = buffer.ReadUInt16(Endianness.Big);
            for (int i = 0; i < itemUseOnceListCount; i++)
            {
                ushort ItemUseOnceListEntry = buffer.ReadUInt16(Endianness.Big);
                ItemUseOnceList.Add(ItemUseOnceListEntry);
            }
        }
    }
}