using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

public class GeneralItem : TlvStructure
{
    private const byte MaxAttrCount = 0x20;

    public GeneralItem()
    {
        ItemId = 0;
        ItemType = 0;
        PosColumn = 0;
        PosGrid = 0;
        Quantity = 0;
        Bind = 0;
        ItemExtAttrIds = new List<byte>();
        ItemExtAttrVals = new List<int>();
    }

    /// <summary>
    /// id to keep track of *this* item
    /// </summary>
    public long ItemId { get; set; }

    /// <summary>
    /// defines the actual item
    /// </summary>
    public int ItemType { get; set; }

    public ItemColumnType PosColumn { get; set; }
    public short PosGrid { get; set; }
    public short Quantity { get; set; }
    public byte Bind { get; set; }
    public byte AttrCount => GetAttrCount();
    public List<byte> ItemExtAttrIds { get; }
    public List<int> ItemExtAttrVals { get; }

    /// <summary>
    /// Convenience property when setting `PosColumn = ItemTabType.Equipment`
    /// to specify actual slot by name
    /// </summary>
    public ItemEquipmentType PosGridEquipment
    {
        get => (ItemEquipmentType)PosGrid;
        set => PosGrid = (short)value;
    }

    public override void Write(IBuffer buffer)
    {
        int startPos = buffer.Position;
        WriteInt32(buffer, 0);

        WriteTlvInt64(buffer, 2, ItemId);
        WriteTlvInt32(buffer, 3, ItemType);
        WriteTlvByte(buffer, 4, (byte)PosColumn);
        WriteTlvInt16(buffer, 5, PosGrid);
        WriteTlvInt16(buffer, 6, Quantity);
        WriteTlvByte(buffer, 7, Bind);

        byte attrCount = GetAttrCount();

        if (attrCount > 0)
        {
            WriteTlvByte(buffer, 8, attrCount);
            WriteTlvInt32(buffer, 10, attrCount);
            for (int i = 0; i < attrCount; i++)
            {
                WriteByte(buffer, ItemExtAttrIds[i]);
            }

            WriteTlvInt32(buffer, 11, attrCount * 4);
            for (int i = 0; i < attrCount; i++)
            {
                WriteInt32(buffer, ItemExtAttrVals[i]);
            }
        }

        int endPos = buffer.Position;
        int entrySize = endPos - startPos - 4;
        buffer.Position = startPos;
        WriteInt32(buffer, entrySize);
        buffer.Position = endPos;
    }

    private byte GetAttrCount()
    {
        byte attrCount = 0;
        int minAttrCount = Math.Min(ItemExtAttrVals.Count, ItemExtAttrIds.Count);
        if (minAttrCount > MaxAttrCount)
        {
            attrCount = MaxAttrCount;
        }

        return attrCount;
    }

    public override void Read(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}