using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

public class Item : Structure, ITlvStructure
{
    private const byte MaxAttrCount = 0x20;

    public Item()
    {
        Id = 0;
        ItemId = 0;
        CharacterId = 0;
        PosColumn = 0;
        PosGrid = 0;
        Quantity = 0;
        Bind = false;
        CreatedBy = null;
        Created = DateTime.MinValue;
        Attributes = new List<ItemAttribute>();
    }

    /// <summary>
    /// Unique Id to keep track of item
    /// </summary>
    public ulong Id { get; set; }

    /// <summary>
    /// Id to reference static item data
    /// </summary>
    public uint ItemId { get; set; }

    public uint CharacterId { get; set; }
    public ItemColumnType PosColumn { get; set; }
    public ushort PosGrid { get; set; }

    /// <summary>
    /// Convenience property when setting `PosColumn = ItemTabType.Equipment`
    /// to specify actual slot by name
    /// </summary>
    public ItemEquipmentType PosGridEquipment
    {
        get => (ItemEquipmentType)PosGrid;
        set => PosGrid = (ushort)value;
    }

    public ushort Quantity { get; set; }
    public bool Bind { get; set; }
    public string CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public List<ItemAttribute> Attributes { get; }

    public void WriteTlv(IBuffer buffer)
    {
        int startPos = buffer.Position;
        WriteInt32(buffer, 0);

        WriteTlvUInt64(buffer, 2, Id);
        WriteTlvUInt32(buffer, 3, ItemId);
        WriteTlvByte(buffer, 4, (byte)PosColumn);
        WriteTlvUInt16(buffer, 5, PosGrid);
        WriteTlvUInt16(buffer, 6, Quantity);
        WriteTlvBool(buffer, 7, Bind);

        byte attrCount = (byte)Attributes.Count;

        if (attrCount > 0)
        {
            WriteTlvByte(buffer, 8, attrCount);
            WriteTlvInt32(buffer, 10, attrCount);
            for (int i = 0; i < attrCount; i++)
            {
                WriteByte(buffer, Attributes[i].Id);
            }

            WriteTlvInt32(buffer, 11, attrCount * 4);
            for (int i = 0; i < attrCount; i++)
            {
                WriteInt32(buffer, Attributes[i].Value);
            }
        }

        int endPos = buffer.Position;
        int entrySize = endPos - startPos - 4;
        buffer.Position = startPos;
        WriteInt32(buffer, entrySize);
        buffer.Position = endPos;
    }

    public void ReadTlv(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}