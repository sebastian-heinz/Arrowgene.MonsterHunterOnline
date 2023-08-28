using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;
using Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

namespace Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

public class Inventory
{
    private const int NoFreeSlot = -1;
    private const int MaxItemSize = 100;

    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(Inventory));

    private Item[] _item;
    private Item[] _store;
    private Item[] _box;
    private Item[] _equipment;
    private Item[] _quest;
    private object _lock;


    public Inventory()
    {
        _item = new Item[MaxItemSize];
        _store = new Item[MaxItemSize];
        _box = new Item[MaxItemSize];
        _equipment = new Item[MaxItemSize];
        _quest = new Item[MaxItemSize];
        _lock = new object();
    }

    public bool Add(Item item)
    {
        switch (item.PosColumn)
        {
            case ItemColumnType.Item: return Add(item, _item);
            case ItemColumnType.Store: return Add(item, _store);
            case ItemColumnType.BoxEquip: return Add(item, _box);
            case ItemColumnType.Equipment: return Add(item, _equipment);
            case ItemColumnType.Quest: return Add(item, _quest);
        }

        return false;
    }

    public bool Move(ulong itemId,
        ItemColumnType srcColumn,
        ushort srcGrid,
        ItemColumnType dstColumn,
        ushort dstGrid)
    {
        return true;
    }

    public void PopulateItemListProperties(IItemListProperties properties)
    {
        properties.NormalSize = 30;
        properties.MaterialStoreSize = 30;
        properties.StoreSize = 30;
        properties.EquipItem.UnknownA = 1;
        properties.EquipItem.UnknownB = 1;
        properties.EquipItem.Items.Add(new GeneralItem()
        {
            ItemId = 1,
            PosColumn = ItemColumnType.Equipment,
            PosGridEquipment = ItemEquipmentType.Weapon,
            ItemType = 120005,
            Quantity = 1,
        });
        properties.EquipItem.Items.Add(new GeneralItem()
        {
            ItemId = 2,
            PosColumn = ItemColumnType.Equipment,
            PosGridEquipment = ItemEquipmentType.Helmet,
            ItemType = 60011,
            Quantity = 1,
        });
        properties.BagItem.Items.Add(new GeneralItem()
        {
            ItemId = 3,
            PosColumn = ItemColumnType.BoxEquip,
            PosGrid = 0,
            ItemType = 55784,
            Quantity = 1,
        });
        properties.BagItem.Items.Add(new GeneralItem()
        {
            ItemId = 4,
            PosColumn = ItemColumnType.BoxEquip,
            PosGrid = 1,
            ItemType = 120006,
            Quantity = 1,
        });
    }

    private bool Add(Item item, Item[] collection)
    {
        int freeSlot = GetFreeSlot(collection);
        if (freeSlot == NoFreeSlot)
        {
            Logger.Error("no free slot");
            return false;
        }

        // TODO
        collection[freeSlot] = item;
        item.Id = freeSlot;
        item.PosGrid = (short)freeSlot;
        return true;
    }

    private int GetFreeSlot(Item[] collection)
    {
        for (int i = 0; i < collection.Length; i++)
        {
            if (collection[i] == null)
            {
                return i;
            }
        }

        return NoFreeSlot;
    }
}