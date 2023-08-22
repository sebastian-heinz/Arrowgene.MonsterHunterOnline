using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.System.Inventory;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

public class TlvItem : TlvStructure
{
    private const int UnknownAMaxSize = 0x20;
    private const int UnknownBMaxSize = 0x20;

    public TlvItem()
    {
        Id = 0;
        ItemId = 0;
        TabType = ItemTabType.Item;
        EquipmentType = ItemEquipmentType.Weapon;
        Slot = 0;
        Quantity = 0;
        UnknownC = 0;
        UnknownD = 0;
        UnknownA = new List<byte>();
        UnknownB = new List<int>();
    }

    public ulong Id { get; set; }
    public uint ItemId { get; set; }
    public ItemTabType TabType { get; set; }

    public ItemEquipmentType EquipmentType { get; set; }
    public ushort Slot { get; set; }
    public ushort Quantity { get; set; }
    public byte UnknownC { get; set; }
    public byte UnknownD { get; set; }
    public List<byte> UnknownA { get; }
    public List<int> UnknownB { get; }

    public override void Write(IBuffer buffer)
    {
        int startPos = buffer.Position;
        WriteInt32(buffer, 0);

        // case 1
        WriteTdrTlvTag(buffer, 2, TlvType.ID_8_BYTE);
        WriteUInt64(buffer, Id);

        // case 2
        WriteTdrTlvTag(buffer, 3, TlvType.ID_4_BYTE);
        WriteUInt32(buffer, ItemId);

        // case 3
        WriteTdrTlvTag(buffer, 4, TlvType.ID_1_BYTE);
        WriteByte(buffer, (byte)TabType);

        // case 4
        WriteTdrTlvTag(buffer, 5, TlvType.ID_2_BYTE);
        if (TabType == ItemTabType.Equipment)
        {
            WriteUInt16(buffer, (byte)EquipmentType);
        }
        else
        {
            WriteUInt16(buffer, Slot);
        }

        // case 5
        WriteTdrTlvTag(buffer, 6, TlvType.ID_2_BYTE);
        WriteUInt16(buffer, Quantity);

        // case 6
        WriteTdrTlvTag(buffer, 7, TlvType.ID_1_BYTE);
        WriteByte(buffer, UnknownC);

        // case 7
        WriteTdrTlvTag(buffer, 8, TlvType.ID_1_BYTE);
        WriteByte(buffer, UnknownD);

        // case 8
        // skip bytes

        // case 9
        if (UnknownA.Count > 0)
        {
            int size = Math.Min(UnknownA.Count, UnknownAMaxSize);
            WriteTdrTlvTag(buffer, 10, TlvType.ID_4_BYTE);
            WriteInt32(buffer, size);
            for (int i = 0; i < size; i++)
            {
                WriteByte(buffer, UnknownA[i]);
            }
        }

        // case 10
        if (UnknownB.Count > 0)
        {
            int size = Math.Min(UnknownA.Count, UnknownBMaxSize);
            WriteTdrTlvTag(buffer, 11, TlvType.ID_4_BYTE);
            WriteInt32(buffer, size * 4);
            for (int i = 0; i < size; i++)
            {
                WriteInt32(buffer, UnknownA[i]);
            }
        }

        int endPos = buffer.Position;
        int entrySize = endPos - startPos - 4;
        buffer.Position = startPos;
        WriteInt32(buffer, entrySize);
        buffer.Position = endPos;
    }

    public override void Read(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}