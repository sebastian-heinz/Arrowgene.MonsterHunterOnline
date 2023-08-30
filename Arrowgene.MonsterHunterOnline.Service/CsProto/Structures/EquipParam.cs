using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 换装参数
    /// </summary>
    public class EquipParam : Structure, ICsStructure
    {
        public EquipParam()
        {
            SrcColumn = 0;
            SrcGrid = 0;
            ItemId = 0;
            DstColumn = 0;
            DstGrid = 0;
        }

        /// <summary>
        /// 栏位
        /// </summary>
        public ItemColumnType SrcColumn { get; set; }

        /// <summary>
        /// 格子id
        /// </summary>
        public int SrcGrid { get; set; }

        /// <summary>
        /// 物品id
        /// </summary>
        public ulong ItemId { get; set; }

        /// <summary>
        /// 栏位
        /// </summary>
        public ItemColumnType DstColumn { get; set; }

        /// <summary>
        /// 格子id
        /// </summary>
        public int DstGrid { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteUInt16(buffer, (ushort)SrcColumn);
            WriteInt32(buffer, SrcGrid);
            WriteUInt64(buffer, ItemId);
            WriteUInt16(buffer, (ushort)DstColumn);
            WriteInt32(buffer, DstGrid);
        }

        public void ReadCs(IBuffer buffer)
        {
            SrcColumn = (ItemColumnType)ReadUInt16(buffer);
            SrcGrid = ReadInt32(buffer);
            ItemId = ReadUInt64(buffer);
            DstColumn = (ItemColumnType)ReadUInt16(buffer);
            DstGrid = ReadInt32(buffer);
        }
    }
}