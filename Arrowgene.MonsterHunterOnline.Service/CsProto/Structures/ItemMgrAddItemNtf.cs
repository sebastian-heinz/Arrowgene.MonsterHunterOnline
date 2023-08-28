using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 通知客户端添加物品
    /// </summary>
    public class ItemMgrAddItemNtf : Structure
    {
        public ItemMgrAddItemNtf()
        {
            Reason = 0;
            ItemList = new List<ItemData>();
        }

        /// <summary>
        /// 原因
        /// </summary>
        public ushort Reason { get; set; }

        /// <summary>
        /// 物品列表
        /// </summary>
        public List<ItemData> ItemList { get; }

        public override void Write(IBuffer buffer)
        {
            byte itemSize = Math.Min((byte)ItemList.Count, (byte)CsProtoConstant.CS_MAX_ITEMMGR_ITEM_COUNT);
            WriteByte(buffer, itemSize);
            WriteUInt16(buffer, Reason);
            WriteList(buffer, ItemList, itemSize, CsProtoConstant.CS_MAX_ITEMMGR_ITEM_COUNT, WriteStructure);
        }

        public override void Read(IBuffer buffer)
        {
            byte itemSize = ReadByte(buffer);
            Reason = ReadUInt16(buffer);
            ReadList(buffer, ItemList, itemSize, CsProtoConstant.CS_MAX_ITEMMGR_ITEM_COUNT, ReadStructure<ItemData>);
        }
    }
}