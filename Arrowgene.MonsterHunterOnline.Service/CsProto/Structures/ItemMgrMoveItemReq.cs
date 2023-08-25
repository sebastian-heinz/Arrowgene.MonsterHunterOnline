using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 请求移动物品
    /// </summary>
    public class ItemMgrMoveItemReq : Structure
    {
        public ItemMgrMoveItemReq()
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
        public ulong ItemId { get; set; }

        /// <summary>
        /// 物品栏位
        /// </summary>
        public byte ItemColumn { get; set; }

        /// <summary>
        /// 物品格子
        /// </summary>
        public ushort ItemGrid { get; set; }

        /// <summary>
        /// 目标栏位
        /// </summary>
        public byte DstColumn { get; set; }

        /// <summary>
        /// 目标格子
        /// </summary>
        public ushort DstGrid { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteUInt64(buffer, ItemId);
            WriteByte(buffer, ItemColumn);
            WriteUInt16(buffer, ItemGrid);
            WriteByte(buffer, DstColumn);
            WriteUInt16(buffer, DstGrid);
        }

        public override void Read(IBuffer buffer)
        {
            ItemId = ReadUInt64(buffer);
            ItemColumn = ReadByte(buffer);
            ItemGrid = ReadUInt16(buffer);
            DstColumn = ReadByte(buffer);
            DstGrid = ReadUInt16(buffer);
        }
    }
}